import React, { useEffect, useState } from 'react'
import TodoItem from './components/TodoItem'
import AddTodo from './components/AddTodo'
import { getTodos, addTodo, deleteTodo } from './API'
import Modal from 'react-modal';


const App: React.FC = () => {
  const [todos, setTodos] = useState<ITodo[]>([])
  const [todoAdded, setTodoAdded] = useState(false)
  const [modalIsOpen,setIsOpen] = useState(false)
  const [selectedTodo, setSelectedTodo] = useState(null)

  const customStyles = {
    content : {
      top                   : '50%',
      left                  : '50%',
      right                 : 'auto',
      bottom                : 'auto',
      marginRight           : '-50%',
      transform             : 'translate(-50%, -50%)'
    }
  };

  const openModal = (todo: ITodo) => {
    setIsOpen(true);
    // setSelectedTodo(todo);
  }

  function closeModal(){
    setIsOpen(false);
  }

  const fetchTodos = (): void => {
    getTodos()
    .then(
      (todos: ITodo[] | any) => {
        console.log(todos);
        setTodos(todos.data)})
    .catch((err: Error) => console.log(err))
  }

 const handleSaveTodo = (e: React.FormEvent, formData: ITodo): void => {
   e.preventDefault()
   addTodo(formData)
   .then((response) => {
     console.log(response)
    // if (status !== 201) {
    //   throw new Error('Error! Todo not saved')
    // }
    setTodoAdded(!todoAdded)
  })
  .catch((err) => console.log(err))
}

  // const handleUpdateTodo = (todo: ITodo): void => {
  //   updateTodo(todo)
  //   .then(({ status, data }) => {
  //       if (status !== 200) {
  //         throw new Error('Error! Todo not updated')
  //       }
  //       setTodos(data.todos)
  //     })
  //     .catch((err) => console.log(err))
  // }

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

  
  useEffect(() => {
    fetchTodos()
  }, [todoAdded])

  console.log(todos);
  return (
    <main className='App'>
      <h1>TodoList</h1>
      <AddTodo saveTodo={handleSaveTodo} />
      {todos.map((todo: ITodo) => (
        <TodoItem
          key={todo.id}
          deleteTodo={handleDeleteTodo}
          //updateTodo= {openModal}
          todo={todo}

        />
      ))}
       <Modal
          isOpen={modalIsOpen}
          onRequestClose={closeModal}
          contentLabel="Example Modal"
          style={customStyles}

        >
          <div>
            {/* <AddTodo updateTodo={updateTodoHandler}/> */}
          </div>
        </Modal>

    </main>
  )
}

export default App
