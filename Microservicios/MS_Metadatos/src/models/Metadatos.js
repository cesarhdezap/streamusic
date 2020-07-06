'use strict'
const mongoose = require('../config/mongoose');

const Schema = mongoose.Schema;

var MetadatoSchema = new Schema({ 

    IdCancion: {type: String, required: true},
    IdConsumidor: {type: String, required: true},
    CantidadDeVecesEscuchada: {type: Number, required: true},
    FechaPrimeraEscucha: { type: Date, default: Date.now },
    FechaUltimaEscucha: { type: Date, default: Date.now },
    Calificacion: { type: Number, required: true, max: 5, min: 0 },
    MeGusta: { type: Boolean, default: false}

}, {collection: 'Metadatos' });

const model = mongoose.model('Metadatos', MetadatoSchema);
module.exports = model;