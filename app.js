import express from 'express';
import appdb from "./database/appdb";

//Setup the Express appdb
const app = express();

//get all todos
app.get('api/v1/todos', (req, res) => {
    res.status(200).send({
        success: 'true',
        message: 'todos retrieved successfully',
        todos: appdb
    })
});

const port = 3000;

app.listen(port, () => {
    console.log(`server running on port ${PORT}`);
});