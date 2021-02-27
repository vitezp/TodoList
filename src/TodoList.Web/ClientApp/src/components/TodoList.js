import React, { Component } from 'react';

export class TodoList extends Component {
    static displayName = TodoList.name;

    constructor(props) {
        super(props);
        this.state = { todoItems: [], loading: true };
    }

    componentDidMount() {
        this.populateTodoListItems();
    }

    static renderTodoList(todoItems) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                <tr>
                    <th>Name</th>
                    <th>Status</th>
                    <th>Priority</th>
                </tr>
                </thead>
                <tbody>
                {todoItems.map(todoItems =>
                    <tr key={todoItems.Name}>
                        <td>{todoItems.Name}</td>
                        <td>{todoItems.Status}</td>
                        <td>{todoItems.Priority}</td>
                    </tr>
                )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : TodoList.renderTodoList(this.state.forecasts);

        return (
            <div>
                <h1 id="tabelLabel" >TodoList</h1>
                <p>This component demonstrates dummy todolist table.</p>
                {contents}
            </div>
        );
    }

    async populateTodoListItems() {
        const response = await fetch(`${process.env.REACT_APP_API_URL}/TodoItem/api/v1/todo`);
        const data = await response.json();
        this.setState({ todoItems: data, loading: false });
    }
}
