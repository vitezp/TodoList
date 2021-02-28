import React, {
    useState,
    useEffect
} from 'react';
import TodoItem from './TodoItem';
import './Todo.css';

const Todo = () => {
    const [todos, setTodos] = useState([{}])
    const [todo, setTodo] = useState({
        name: '',
        status: '',
        priority: ''
    })

    async function fetchAPI() {
        let response = await fetch('https://localhost:5001/TodoItem/api/v1/todo');
        response = await response.json();

        setTodos(response);
    }

    const handleAdd = event => {
        event.preventDefault();
        console.log(JSON.stringify(todo))


        fetch('https://localhost:5001/TodoItem/api/v1/todo', {
            method: 'POST',
            headers: {
                'Content-Type': 'text/json',
                'accept': 'text/plain'
            },
            body: JSON.stringify(todo)
        }).then(function (response) {
            if (response.ok) {
                fetchAPI()
                return response.json();
            }
            return Promise.reject(response);
        }).then(function (data) {
            console.log(data);
        }).catch(function (error) {
            alert('Something went wrong.', error);
        });
    }

    const onChangeHandler = event => {
        setTodo({
            ...todo,
            [event.target.name]: event.target.value
        })
    }

    const handleDelete = (todo) => {
        console.log('deleting' + todo.id);

        fetch(`https://localhost:5001/TodoItem/api/v1/todo/${todo.id}`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'text/json',
                'accept': 'text/plain'
            }
        }).then(function (response) {
            if (response.ok) {
                fetchAPI()
                return response.json();
            }
            return Promise.reject(response);
        }).then(function (data) {
            console.log(data);
        }).catch(function (error) {
            console.warn('Something went wrong.', error);
        });
    }

    const handleUpdate = (todo) => {
        console.log('updating' + todo.id);

        fetch(`https://localhost:5001/TodoItem/api/v1/todo`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'text/json',
                'accept': 'text/plain',
            },
            body: JSON.stringify(todo)
        }).then(function (response) {
            if (response.ok) {
                fetchAPI()
                return response.json();
            }
            return Promise.reject(response);
        }).then(function (data) {
            console.log(data);
        }).catch(function (error) {
            console.warn('Something went wrong.', error);
        });
    }


    useEffect(() => {
        fetchAPI()
    }, []);

    return (
        <div className="container">
            <ul>
                {todos.map((todoItem) => {
                    return <li key={todoItem.id} id={todoItem.id}><TodoItem name={todoItem.name}
                                                                            status={todoItem.status}
                                                                            priority={todoItem.priority}/>
                        <button type="button" onClick={() => handleDelete(todoItem)}>
                            Remove
                        </button>
                        <button type="button" onClick={() => handleUpdate(todoItem)}>
                            Edit
                        </button>
                    </li>
                })}
            </ul>

            <div className="addTodo">
                <form onSubmit={handleAdd}>
                    <input type="text" name="name" value={todo.name} onChange={onChangeHandler}></input>
                    <select name="status" onChange={onChangeHandler}>
                        <option value="Completed">Completed</option>
                        <option value="InProgress">InProgress</option>
                        <option value="NotStarted" selected>NotStarted</option>
                    </select>
                    <input type="text" name="priority" value={todo.priority} onChange={onChangeHandler}></input>

                    <button type="submit">Add Todo</button>
                </form>
            </div>


        </div>
    );
}

export default Todo;