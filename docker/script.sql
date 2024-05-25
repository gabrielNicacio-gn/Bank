CREATE UNLOGGED TABLE accounts(
        id SERIAL PRIMARY KEY,
        fullname VARCHAR(100) NOT NULL,
        email VARCHAR(50) NOT NULL,
        cpf VARCHAR(11) NOT NULL,
        balance DECIMAL NOT NULL,
        password VARCHAR(20) NOT NULL,
        UNIQUE(cpf,email)
);

CREATE UNLOGGED TABLE transactions(
        id SERIAL PRIMARY KEY,
        id_receiver INT NOT NULL,
        id_sender INT NOT NULL,
        value DECIMAL NOT NULL,
        date TIMESTAMP NOT NULL,
        FOREIGN KEY(id_sender) REFERENCES accounts(id),
        FOREIGN KEY(id_receiver) REFERENCES accounts(id) 
);

INSERT INTO accounts (fullname,email,cpf,balance,password) VALUES
('User One','userone@example.com','23456789010',100,'user123'),
('User Two','usertwo@example.com','09876543211',100,'user456')
