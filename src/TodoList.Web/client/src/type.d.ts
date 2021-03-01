interface ITodo {
    id: number
    name: string
    status: string
    priority: number
    errorResponse: object
}

type TodoProps = {
    todo: ITodo
}

type ApiDataType = {
    todos: ITodo[]
    todo?: ITodo
}
  