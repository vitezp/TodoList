import React from 'react'

type Props = TodoProps & {
    deleteTodo: (id: number) => void
    setEditing: any
}

const Todo: React.FC<Props> = ({ todo, deleteTodo, setEditing }) => {
  const checkTodo: string = todo.status === 'Completed' ? `line-through` : ''
  return (
    <div className='Card'>
      <div className='Card--text'>
        <h1 className={checkTodo}>{todo.name}</h1>
        <div className='d-flex-row'>
          <span className={checkTodo}>{todo.status}</span>
          <span className={checkTodo}>Priority: {todo.priority}</span>
        </div>
        
      </div>
      <div className='Card--button'>
        <button
          onClick={() => setEditing(todo)}
          className={todo.status === 'Completed' ? `hide-button` : 'Card--button__done'}
        >
          Edit
        </button>
       
        <button
          onClick={() => deleteTodo(todo.id)}
          className='Card--button__delete'
        >
          Delete
        </button>
      </div>
    </div>
  )
}

export default Todo
