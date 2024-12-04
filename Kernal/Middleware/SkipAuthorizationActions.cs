namespace SharedKernel
{
    public static class SkipAuthorizationActions
    {
        // Holds pairs of controller names and action names to skip
        private static readonly HashSet<string> _skipActions = new HashSet<string>();

        // Initially contains only "Login" in the "Auth" controller for skipping authorization
        static SkipAuthorizationActions()
        {
            // Example: skip authorization for "Login" action in "Auth" controller
            _skipActions.Add("Auth.Login");
        }

        // Check if the combination of controller and action should be skipped
        public static bool ShouldSkip(string controller, string action)
        {
            return _skipActions.Contains($"{controller}.{action}");
        }

        // Method to add a controller-action pair to the skip list
        public static void AddToSkipList(string controller, string action)
        {
            _skipActions.Add($"{controller}.{action}");
        }
    }
}
