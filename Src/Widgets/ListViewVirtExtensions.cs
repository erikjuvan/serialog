namespace serialog
{
    public static class ListViewVirtExtensions
    {
        public static bool UseRegex = false;
        public static bool MatchCase = false;

        public static bool FindNext(
            this ListViewVirt listView,
            string text)
        {
            if (string.IsNullOrEmpty(text) || listView.DataLog.Count == 0)
                return false;

            int startIndex = listView.SelectedIndices.Count > 0
                ? listView.SelectedIndices[^1] + 1
                : 0;

            return listView.SelectMatch(text, startIndex, forward: true);
        }

        public static bool FindPrev(
            this ListViewVirt listView,
            string text)
        {
            if (string.IsNullOrEmpty(text) || listView.DataLog.Count == 0)
                return false;

            int startIndex = listView.SelectedIndices.Count > 0
                ? listView.SelectedIndices[0] - 1
                : listView.DataLog.Count - 1;

            return listView.SelectMatch(text, startIndex, forward: false);
        }

        public static bool FindFirst(
            this ListViewVirt listView,
            string text)
        {
            return listView.SelectMatch(text, 0, forward: true);
        }

        public static bool FindLast(
            this ListViewVirt listView,
            string text)
        {
            return listView.SelectMatch(text, listView.DataLog.Count - 1, forward: false);
        }

        public static bool FindAll(
            this ListViewVirt listView,
            string text)
        {
            if (string.IsNullOrEmpty(text) || listView.DataLog.Count == 0)
                return false;

            listView.SelectedIndices.Clear();
            bool found = false;

            for (int i = 0; i < listView.DataLog.Count; i++)
            {
                if (Helpers.MatchesPattern(listView.DataLog[i].ToString(), text, MatchCase, UseRegex))
                {
                    listView.SelectedIndices.Add(i);
                    found = true;
                }
            }

            if (found)
                listView.EnsureVisible(listView.SelectedIndices[^1]);

            return found;
        }

        public static bool FindLive(
            this ListViewVirt listView,
            string text)
        {
            if (string.IsNullOrEmpty(text) || listView.DataLog.Count == 0)
            {
                listView.SelectedIndices.Clear();
                return false;
            }

            int count = listView.DataLog.Count;

            if (listView.SelectedIndices.Count > 0)
            {
                int currentIndex = listView.SelectedIndices[^1];
                string currentLine = listView.DataLog[currentIndex].ToString();

                // 1 Stay if current line still matches
                if (Helpers.MatchesPattern(currentLine, text, MatchCase, UseRegex))
                    return true;

                // 2 Otherwise, search forward from next item
                int nextIndex = (currentIndex + 1) % count;
                if (listView.SelectMatch(text, nextIndex, forward: true))
                    return true;

                // 3 Wrap-around search (only if not already from 0)
                if (nextIndex != 0 && listView.SelectMatch(text, 0, forward: true))
                    return true;

                // 4 Nothing found — deselect all
                listView.SelectedIndices.Clear();
                return false;
            }

            // 5 No selection yet — start fresh from beginning
            if (listView.SelectMatch(text, 0, forward: true))
                return true;

            // 6 No match at all — clear selection
            listView.SelectedIndices.Clear();
            return false;
        }

        // shared helper
        private static bool SelectMatch(
            this ListViewVirt listView,
            string text,
            int startIndex,
            bool forward)
        {
            int i = startIndex;
            while (i >= 0 && i < listView.DataLog.Count)
            {
                if (Helpers.MatchesPattern(listView.DataLog[i].ToString(), text, MatchCase, UseRegex))
                {
                    listView.SelectedIndices.Clear();
                    listView.SelectedIndices.Add(i);
                    listView.EnsureVisible(i);
                    return true;
                }
                i += forward ? 1 : -1;
            }
            return false;
        }

        // ///////// //
        // Highlight //
        // ///////// //

        public static bool FindNextHighlighted(this ListViewVirt listView)
        {
            if (listView.DataLog.Count == 0)
                return false;

            int startIndex = listView.SelectedIndices.Count > 0
                ? listView.SelectedIndices[^1] + 1
                : 0;

            return listView.SelectHighlighted(startIndex, forward: true);
        }

        public static bool FindPrevHighlighted(this ListViewVirt listView)
        {
            if (listView.DataLog.Count == 0)
                return false;

            int startIndex = listView.SelectedIndices.Count > 0
                ? listView.SelectedIndices[0] - 1
                : listView.DataLog.Count - 1;

            return listView.SelectHighlighted(startIndex, forward: false);
        }

        public static bool FindFirstHighlighted(this ListViewVirt listView) =>
            listView.SelectHighlighted(0, forward: true);

        public static bool FindLastHighlighted(this ListViewVirt listView) =>
            listView.SelectHighlighted(listView.DataLog.Count - 1, forward: false);

        private static bool SelectHighlighted(this ListViewVirt listView, int startIndex, bool forward)
        {
            int i = startIndex;
            while (i >= 0 && i < listView.DataLog.Count)
            {
                var highlight = FormHighlight.Highlights.FindMatch(listView.DataLog[i].ToString());
                if (highlight != null && !highlight.Hide)
                {
                    listView.SelectedIndices.Clear();
                    listView.SelectedIndices.Add(i);
                    listView.EnsureVisible(i);
                    return true;
                }
                i += forward ? 1 : -1;
            }
            return false;
        }
    }
}
