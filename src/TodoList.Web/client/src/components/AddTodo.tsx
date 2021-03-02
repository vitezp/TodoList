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

  const clearFormData = (): void => {
        setFormData({})
  }


  return (
    <form className='Form' onSubmit={(e) => {saveTodo(e, formData);clearFormData()}}>
      <div className='form-row'>
        <div>
          <label htmlFor='name'>Name</label>
          <input onChange={handleForm} type='text' id='name' />
        </div>
        <div>
          <label htmlFor='priority'>Priority</label>
          <input onChange={handleForm} type='text' id='priority' />
        </div>
        <div>
          <label htmlFor='status'>Status</label>
            <div className='select'>
              <select name='status' defaultValue='NotStarted' id='status' onChange={handleForm}>
                <option value='Completed'>Completed</option>
                <option value='InProgress'>In Progress</option>
                <option value='NotStarted'>Not started</option>
              </select>
            </div>
        </div>
      </div>
      <button disabled={formData === undefined ? true: false} >Add Todo</button>
    </form>
  )
}

export default AddTodo
