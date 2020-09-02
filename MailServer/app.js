'use strict';

const express = require('express');
const bodyParser = require('body-parser');
const mailHandler = require('./mailhandler');

const app = express();

app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: true }));

app.post('/send', mailHandler);

app.listen(8080, () => console.log('Servico de emails pronto!'));