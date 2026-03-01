-- Таблица заказчиков (клиенты)
CREATE TABLE Clients (
    clientID INT PRIMARY KEY,
    fio NVARCHAR(100) NOT NULL,
    phone NVARCHAR(20) NOT NULL,
    login NVARCHAR(50) NOT NULL,
    password NVARCHAR(50) NOT NULL
);

-- Таблица мастеров (технари)
CREATE TABLE Masters (
    masterID INT PRIMARY KEY,
    fio NVARCHAR(100) NOT NULL,
    phone NVARCHAR(20) NOT NULL,
    login NVARCHAR(50) NOT NULL,
    password NVARCHAR(50) NOT NULL,
    position NVARCHAR(50) NOT NULL
);

-- Таблица статусов заявок
CREATE TABLE RequestStatuses (
    statusID INT PRIMARY KEY,
    statusName NVARCHAR(50) NOT NULL
);

-- Таблица типов устройств
CREATE TABLE DeviceTypes (
    typeID INT PRIMARY KEY,
    typeName NVARCHAR(50) NOT NULL
);

-- Таблица заявок (главная таблица)
CREATE TABLE Requests (
    requestID INT PRIMARY KEY,
    startDate DATE NOT NULL,
    deviceTypeID INT NOT NULL,
    deviceModel NVARCHAR(100) NOT NULL,
    problemDescription NVARCHAR(500) NOT NULL,
    statusID INT NOT NULL,
    completionDate DATE NULL,
    repairParts NVARCHAR(500) NULL,
    masterID INT NULL,
    clientID INT NOT NULL,
    -- Связи
    FOREIGN KEY (deviceTypeID) REFERENCES DeviceTypes(typeID),
    FOREIGN KEY (statusID) REFERENCES RequestStatuses(statusID),
    FOREIGN KEY (masterID) REFERENCES Masters(masterID),
    FOREIGN KEY (clientID) REFERENCES Clients(clientID)
);

-- Таблица комментариев
CREATE TABLE Comments (
    commentID INT PRIMARY KEY,
    message NVARCHAR(500) NOT NULL,
    masterID INT NOT NULL,
    requestID INT NOT NULL,
    -- Связи
    FOREIGN KEY (masterID) REFERENCES Masters(masterID),
    FOREIGN KEY (requestID) REFERENCES Requests(requestID)
);