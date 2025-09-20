namespace serialog
{
    public static class ListViewVirtExtensions
    {
        public static bool UseRegex = false;

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
                if (Helpers.MatchesPattern(listView.DataLog[i].GetContent(), text, UseRegex))
                {
                    listView.SelectedIndices.Add(i);
                    found = true;
                }
            }

            if (found)
                listView.EnsureVisible(listView.SelectedIndices[^1]);

            return found;
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
                if (Helpers.MatchesPattern(listView.DataLog[i].GetContent(), text, UseRegex))
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
