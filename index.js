const express = require('express')
const app = express()
const port = 3000 //for local testing until I deploy to Heroku - Zach

app.get('/',(request,response)=>{
    response.send('Hello from express')
})

app.listen(port, (err)=>{
    if err return console.log('error', err)

    console.log('server is listening on ${port}')
})
