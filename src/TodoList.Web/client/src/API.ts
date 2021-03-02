import axios, { AxiosResponse } from 'axios'

const baseUrl: string = 'https://localhost:5001/TodoItem/api/v1/todo'

export const getTodos = async (): Promise<AxiosResponse<ApiDataType>> => {
  try {
    const todos: AxiosResponse<ApiDataType> = await axios.get(
      baseUrl
    )
    return todos
  } catch (error) {
    throw new Error(error)
  }
}

export const addTodo = async (
  formData: ITodo
): Promise<AxiosResponse<ApiDataType>> => {
  try {
    const todo: Omit<ITodo, 'id' | 'errorResponse'> = {
      name: formData.name,
      priority : formData.priority,
      status: formData.status
    }
    const saveTodo: AxiosResponse<ApiDataType> = await axios.post(
      baseUrl,
      todo
    )
    return saveTodo
  } catch (error) {
    throw new Error(error)
  }
}

export const updateTodo = async (
  formData: ITodo
): Promise<AxiosResponse<ApiDataType>> => {
  try {
    const todoUpdate: Pick<ITodo, 'id'|'status'| 'priority'| 'name'> = {
      id: formData.id,
      status: formData.status,
      priority: formData.priority,
      name: formData.name
    }
    const updatedTodo: AxiosResponse<ApiDataType> = await axios.put(
      baseUrl,
      todoUpdate
    )
    
    return updatedTodo
  } catch (error) {
    throw new Error(error)
  }
}

export const deleteTodo = async (
  _id: number
): Promise<AxiosResponse<ApiDataType>> => {
  try {
    const deletedTodo: AxiosResponse<ApiDataType> = await axios.delete(
      `${baseUrl}/${_id}`
    )
    return deletedTodo
  } catch (error) {
    throw new Error(error)
  }
}
