using System.IO.Pipes;

namespace serialog
{
    public class PipeServer
    {
        private readonly string pipeName;
        private readonly Action<string> onCommand;
        private CancellationTokenSource cts;

        public PipeServer(string pipeName, Action<string> onCommand)
        {
            this.pipeName = pipeName;
            this.onCommand = onCommand;
        }

        public void Start()
        {
            cts = new CancellationTokenSource();
            Task.Run(() => RunServerLoop(cts.Token));
        }

        public void Stop()
        {
            if (cts == null) return;

            if (!cts.IsCancellationRequested)
            {
                try
                {
                    cts.Cancel();
                }
                catch (ObjectDisposedException)
                {
                    // Ignore - already disposed
                }
            }
        }

        private async Task RunServerLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                using (var server = new NamedPipeServerStream(pipeName, PipeDirection.In, 1,
                           PipeTransmissionMode.Message, PipeOptions.Asynchronous))
                {
                    try
                    {
                        await server.WaitForConnectionAsync(token);
                        using (var reader = new StreamReader(server))
                        {
                            string command = await reader.ReadLineAsync();
                            if (!string.IsNullOrWhiteSpace(command))
                                onCommand?.Invoke(command.Trim().ToLowerInvariant());
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    catch (ObjectDisposedException)
                    {
                        break; // clean exit on disposal
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"[PipeServer] Error: {ex.Message}");
                    }
                }
            }
        }
    }
}
