import React, { useEffect, useState } from 'react'
import TodoItem from './components/TodoItem'
import AddTodo from './components/AddTodo'
import EditTodo from './components/EditTodo'
import { getTodos, addTodo, updateTodo, deleteTodo } from './API'


const App: React.FC = () => {
  const [todos, setTodos] = useState<ITodo[]>([])
  const [todoAdded, setTodoAdded] = useState(false)
  const [editing, setEditing] = useState(false)

  const [currentTodo, setCurrentTodo] = useState({})

  const fetchTodos = (): void => {
    getTodos()
    .then(
      (todos: ITodo[] | any) => {
        setTodos(todos.data)})
    .catch((err: Error) => console.log(err))
  }

 const handleSaveTodo = (e: React.FormEvent, formData: ITodo): void => {
   e.preventDefault()
   addTodo(formData)
   .then((response) => {
     if (response.status !== 201) {
      alert("Unable to add todo item") 
      throw new Error('Error! Todo not saved')
     }
    setTodoAdded(!todoAdded)
  })
  .catch((err) => console.log(err))
}

  const handleDeleteTodo = (_id: number): void => {
    deleteTodo(_id)
    .then(({ status, data }) => {
        if (status !== 204) {
          throw new Error('Error! Todo not deleted')
        }
        setTodoAdded(!todoAdded)
      })
      .catch((err) => console.log(err))
  }

  const editTodo = (todo: ITodo | any): void => {
    setEditing(true)

    setCurrentTodo({id:todo.id, name: todo.name, status: todo.status, priority: todo.priority})
  } 

  const handleUpdateTodo = (updatedTodo: ITodo): void => {
    setEditing(false);

    updateTodo(updatedTodo)
    .then(({ status, data }) => {
      if (status !== 201) {
        throw new Error('Error! Todo not added')
      }
      setTodoAdded(!todoAdded)
    })
    .catch((err) => console.log(err))

  }
 
  useEffect(() => {
    fetchTodos()
  }, [todoAdded])

  return (
    <main className='App'>
      <h1>TodoList</h1>
      { editing ? <EditTodo todo={currentTodo} updateTodo={handleUpdateTodo} setEditing={setEditing}/> : <AddTodo saveTodo={handleSaveTodo} />
      }
      {todos.map((todo: ITodo) => (
        <TodoItem
          key={todo.id}
          deleteTodo={handleDeleteTodo}
          todo={todo}
          setEditing={editTodo}
        />
      ))}

    </main>
  )
}

export default App
