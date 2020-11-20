const nodeMailer = require('nodemailer');

var transporter = nodeMailer.createTransport({
    service: 'SendGrid',
    auth: {
        user: 'SomeUser',
        pass: 'SomePassword'
    }
});

function send(req, res) {
    transporter.sendMail({
        from: 'EST Estagios <noreply@apoioestagio.ips.pt>',
        to: req.body.mail,
        replyTo: '<noreply@apoioestagio.ips.pt>',
        subject: req.body.subject,
        html: req.body.html
    }, (err, info) => {
            if (err) {
                res.sendStatus(400).end(err);
            } else {
                res.sendStatus(201).end();
            }
    });
}

module.exports = send;
