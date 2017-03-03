const fs = require('fs'); // required to read https certs
const https = require('https');
const express = require('express');
const mongodb = require('mongodb');
const bodyParser = require('body-parser');
const stormpath = require('express-stormpath');
var app = express();

app.use(stormpath.init(app, {
  expand: {
    customData: true
  },
  web: {
    login: {
      enabled: true,
      nextUri: "/"
    }
  }
}))

app.use(bodyParser.json());
app.use(bodyParser.urlencoded({extended: false}))

//const port = 3000
var db;
var localTestUrl = 'mongodb://localhost:27017/test'

mongodb.MongoClient.connect(process.env.MONGODB_URI || localTestUrl, function (err, database) {
  if (err) {
    console.log('ERROR:', err);
    res.redirect('/')
  }

  // Save database object from the callback for reuse.
  db = database;
  console.log("Database connection ready");

  // Initialize the app.
  app.on('stormpath.ready', function() {
    var server = app.listen(process.env.PORT || `8080`, function () {
        var port = server.address().port;
        console.log("App now running on port", port);
    });
  });
});

app.get('/',stormpath.authenticationRequired, (req,res)=>{
    res.send('Welcome to upgrade, ' + req.user.givenName)
});

app.get('/classList', stormpath.authenticationRequired, (req,res)=> {
    //res.type('json');
    var list;
    db.collection('classes', (err, collection)=>{
        if (err) {
          console.log('ERROR:', err);
          res.redirect('/')
        }
        else {
            collection.find({},{name:1, _id:0}).toArray((err, ret)=>{
                if (err){
                    console.log('ERROR:', err)
                    res.redirect('/')
                }
                else {
                    res.send(ret)
                }
            })
        }
    })
})

app.get('/join/:class', stormpath.authenticationRequired, (req,res)=>{
    var list;
    db.collection('classes', (err, collection)=>{
        if (err) {
          console.log('ERROR:', err);
          res.redirect('/')
        }
        else {
            collection.update({_id: ""+req.params.class}, {$pull: {students: ""+req.user.userName}})
        }
    })

    //db.classes.update({_id: req.params.class}, {$pull: {students: req.user.userName}})

    //res.send('Hello, ' + req.user.givenName + ', your username is ' + req.params.username)
})
