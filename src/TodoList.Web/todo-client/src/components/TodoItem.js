import React, {
    useState
} from 'react';
import './TodoItem.css'


const TodoItem = (props) => {

    const [todoState, setTodoState] = useState({
        name: props.name,
        status: props.status,
        priority: props.priority,
        id: props.id
    })

    return ( 
    <div className ="todo-item__wrapper" id={props.id}>
        <span>{props.name}</span>
        <span>{props.status}</span>
        <span>{props.priority}</span>
    </div>
    );
}

export default TodoItem;