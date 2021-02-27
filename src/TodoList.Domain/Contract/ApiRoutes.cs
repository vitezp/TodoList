namespace TodoList.Domain.Contract
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
            public const string Update = Base + "/todo";
            public const string Delete = Base + "/todo/{todoId}";
            public const string GetById = Base + "/todo/{todoId}";
        }
    }
}