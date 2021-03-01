import {useState} from "react";


const TodoItem = ({todoItem}) => {
    const [todoState, setTodoState] = useState({
        name: props.name,
        status: props.status,
        priority: props.priority,
        id: props.id
    })


    const [formData, setFormData] = useState()

    const handleForm = (e) => {
        setFormData({
            ...formData,
            [e.currentTarget.id]: e.currentTarget.value,
        })
    }

    return (
        <form className='Form' onSubmit={(e) => saveTodo(e, formData)}>
            <div>
                <div>
                    <label htmlFor='name'>Name</label>
                    <input onChange={handleForm} type='text' id='name'/>
                </div>
                <div>
                    <label htmlFor='description'>Description</label>
                    <input onChange={handleForm} type='text' id='description'/>
                </div>
            </div>
            <button disabled={formData === undefined ? true : false}>Add Todo</button>
        </form>
    )
}