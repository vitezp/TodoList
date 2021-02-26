namespace TodoList.Shared.Contract
{
    public static class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "v1";
        public const string Base = Root + "/" + Version;

        public static class Todo
        {
            public const string Create = Base + "/todo";
            public const string GetAll = Base + "/todo";
            public const string Update = Base + "/todo/{postId}";
            public const string Delete = Base + "/todo/{postId}";

            public const string Get = Base + "/todo/{postId}";
        }
    }
}