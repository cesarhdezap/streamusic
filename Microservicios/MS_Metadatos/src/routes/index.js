const express = require('express');
const router = express.Router();
const metadataController = require('../controllers/MetadatosController.js');

router.post('/api/postMetadata', metadataController.postMetadata);
router.get('/api/getMetadataAll', metadataController.getMetadataAll);
router.get('/api/getMetadataId/:IdMetadatos', metadataController.getMetadataById);
router.get('/api/Cancion/:IdCancion', metadataController.getMetadataByIdCancion);
router.get('/api/megusta/:IdConsumidor', metadataController.getMetadataLikedByIdConsumidor);
router.get('/api/Consumidor/:IdConsumidor', metadataController.getMetadataByIdConsumidor);
router.delete('/api/deleteMetadataId/:IdMetadatos', metadataController.deleteMetadataById);
router.put('/api/putMetadataId/:IdMetadatos', metadataController.putMetadataById);


module.exports = router;