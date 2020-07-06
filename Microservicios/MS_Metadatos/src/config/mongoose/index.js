'use strict'
const mongoose = require('mongoose');

console.log("DB STRING")
console.log(process.env.DB_CONNECTION_STRING)

mongoose.connect(process.env.DB_CONNECTION_STRING
).then(() => {
    console.log("DB connection exist");
}).catch(
    err => {
        console.log("DB connection failed", err);
    });

module.exports = mongoose;