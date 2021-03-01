import React, { useState } from 'react'

type Props = { 

  saveTodo: (e: React.FormEvent, formData: ITodo | any) => void 
}

const AddTodo: React.FC<Props> = ({ saveTodo }) => {
  const [formData, setFormData] = useState<ITodo | {}>()

  const handleForm = (e: any): void => {
    setFormData({
      ...formData,
      [e.currentTarget.id]: e.currentTarget.value,
    })
  }

  console.log(formData)

  return (
    <form className='Form' onSubmit={(e) => saveTodo(e, formData)}>
      <div>
        <div>
          <label htmlFor='name'>Name</label>
          <input onChange={handleForm} type='text' id='name' />
        </div>
        <div>
          <label htmlFor='priority'>Priority</label>
          <input onChange={handleForm} type='text' id='priority' />
        </div>
        <div>
          <label htmlFor='status'>
            <select name='status' id='status' onChange={handleForm}>
              <option value='Completed'>Completed</option>
              <option value='InProgress'>In Progress</option>
              <option value='NotStarted' selected>Not started</option>
            </select>
          </label>
        </div>
      </div>
      <button disabled={formData === undefined ? true: false} >Add Todo</button>
    </form>
  )
}

export default AddTodo
