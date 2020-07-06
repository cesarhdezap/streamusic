const express = require('express');
const aplicacion = express();
const morgan = require('morgan');
const rutas = require('./routes/index');
const mongoose = require('./config/mongoose/index');

//settings
var port = process.env.PORT || 3000;
aplicacion.set('json spaces', 2);

// middlewares
aplicacion.use(morgan('dev'));
aplicacion.use(express.urlencoded({extended: false}));
aplicacion.use(express.json());

// routes
aplicacion.use(rutas);

aplicacion.listen(port, () => {
        console.log(`Server on port ${aplicacion.get('port')} `);
        mongoose.STATES;
});






