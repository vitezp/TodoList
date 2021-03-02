import React, {useState, useEffect} from 'react'

type Props = { 
    updateTodo: any
    todo: ITodo | any
    setEditing: React.Dispatch<React.SetStateAction<any>>;
  }

const EditTodo: React.FC<Props> = ({todo, updateTodo, setEditing}) => {
    const [editedTodo, setTodo] = useState(todo);

    const handleForm = (event: any): void => {
        console.log(todo)
        setTodo({...editedTodo,  [event.currentTarget.id]: event.currentTarget.value,})
    }

    useEffect(() => {
        setTodo(todo)
    }, [])

    

    return(
        <form className='Form' onSubmit={(e) => updateTodo(editedTodo)}>
            <div>
                <div>
                <label htmlFor='name'>Name</label>
                <input onChange={handleForm} type='text' id='name' defaultValue={editedTodo.name}/>
                </div>
                <div>
                <label htmlFor='priority'>Priority</label>
                <input onChange={handleForm} type='text' id='priority' defaultValue={editedTodo.priority}/>
                </div>
                <div>
                <label htmlFor='status'>Status</label>
                    <div className='select'>
                    <select name='status' defaultValue={editedTodo.status} id='status' onChange={handleForm}>
                        <option value='Completed'>Completed</option>
                        <option value='InProgress'>In Progress</option>
                        <option value='NotStarted'>Not started</option>
                    </select>
                </div>
                </div>
            </div>
            <button type="submit" disabled={todo === undefined ? true: false} >Save changes</button>
    </form>

    )
}
export default EditTodo
