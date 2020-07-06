const Metadata = require('../models/Metadatos.js');

const postMetadata = async(req, res) => {

    let Metadatos = new Metadata()
    Metadatos.IdCancion = req.body.IdCancion
    Metadatos.IdConsumidor = req.body.IdConsumidor
    Metadatos.CantidadDeVecesEscuchada = req.body.CantidadDeVecesEscuchada
    Metadatos.FechaPrimeraEscucha = req.body.FechaPrimeraEscucha
    Metadatos.FechaUltimaEscucha = req.body.FechaUltimaEscucha
    Metadatos.Calificacion = req.body.Calificacion
    Metadatos.MeGusta = false

    Metadatos.save((err, MetadatosSaved) => {
        if(err) res.status(500).send({status: 'error', message: 'Registro de metadatos fallido'})

        res.status(200).send(MetadatosSaved)
    })
}

const getMetadataAll = async(req,res) =>{
    Metadata.find({}, (err, Metadatos)=> {
        if(err) return res.status(500).send({status: 'error', message: 'Error al recuperar los metadatos'})
        if(Metadatos.length < 1) return res.status(404).send({status: 'error', message: 'los metadatos no existen'})

        res.status(200).send(Metadatos)

    })
}

const getMetadataById = async(req, res) =>{
    let IdMetadatos = req.params.IdMetadatos
    Metadata.findById(IdMetadatos, (err,Metadatos) => {
        if (err) return res.status(500).send({status: 'error', message: 'Error al recuperar los metadatos'})
        if(!Metadatos) return res.status(404).send({status: 'error', message: 'los metadatos no existen'})

        res.status(200).send(Metadatos)
    })
}

const deleteMetadataById = async(req, res) =>{
    let IdMetadatos = req.params.IdMetadatos

    Metadata.findById(IdMetadatos, (err, Metadatos) =>{
        if(err) res.status(500).send({status: 'error', message: 'Error al eliminar los metadatos'})

        Metadatos.remove(err =>{
        if(err) res.status(500).send({status: 'error', message: 'Error al eliminar los metadatos'})
            
        res.status(200).send({status: 'ok', menssage: 'Metadatos eliminados con exito'})
        })
    })
}

const putMetadataById = async(req, res) =>{
    let IdMetadatos = req.params.IdMetadatos
    let update = req.body

    Metadata.findByIdAndUpdate(IdMetadatos, update , (err, MetadatosActualizado) =>{
        if(err) res.status(500).send({status: 'error', message: 'Error al actualizar los metadatos'})
        if(!update) return res.status(404).send({status: 'error', message: 'No se actualizo ningun metadato'})
        
        res.status(200).send(MetadatosActualizado)
    })
}

const getMetadataByIdCancion = async(req, res) =>{
    Metadata.findOne({IdCancion: req.params.IdCancion}, (err,Metadatos) =>{
        if (err) return res.status(500).send({status: 'error', message: 'Error al recuperar los metadatos'})
        if(!Metadatos) return res.status(404).send({status: 'error', message: 'los metadatos no existen'})

        res.status(200).send(Metadatos)
    })
}
const getMetadataByIdConsumidor = async(req, res) =>{
    Metadata.find({IdConsumidor: req.params.IdConsumidor}, (err,Metadatos) =>{
        if (err) return res.status(500).send({status: 'error', message: 'Error al recuperar los metadatos'})
        if(Metadatos.length < 1) return res.status(404).send({status: 'error', message: 'los metadatos no existen'})

        res.status(200).send(Metadatos)
    })
}

const getMetadataLikedByIdConsumidor = async (req, res) => {
    Metadata.find({ $or: [{ IdConsumidor: req.params.IdConsumidor, MeGusta: true }] }, (err, Metadatos) => {
        if (err) return res.status(500).send({ status: 'error', message: 'Error al recuperar los metadatos' })
        if (Metadatos.length < 1) return res.status(404).send({ status: 'error', message: 'Los metadatos no existen' })

        res.status(200).send(Metadatos)
    })
}
module.exports = { postMetadata, getMetadataAll, getMetadataById, getMetadataByIdCancion, getMetadataByIdConsumidor, getMetadataLikedByIdConsumidor, deleteMetadataById, putMetadataById};