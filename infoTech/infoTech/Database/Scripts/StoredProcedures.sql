CREATE PROCEDURE FindRequestByNumber
    @RequestNumber INT
AS
BEGIN
    SELECT 
        r.requestID AS 'Номер заявки',
        r.startDate AS 'Дата добавления',
        dt.typeName AS 'Тип устройства',
        r.deviceModel AS 'Модель',
        r.problemDescription AS 'Описание проблемы',
        rs.statusName AS 'Статус',
        r.completionDate AS 'Дата завершения',
        r.repairParts AS 'Запчасти',
        c.fio AS 'Заказчик',
        c.phone AS 'Телефон заказчика',
        m.fio AS 'Мастер',
        m.position AS 'Должность мастера'
    FROM Requests r
    LEFT JOIN DeviceTypes dt ON r.deviceTypeID = dt.typeID
    LEFT JOIN RequestStatuses rs ON r.statusID = rs.statusID
    LEFT JOIN Clients c ON r.clientID = c.clientID
    LEFT JOIN Masters m ON r.masterID = m.masterID
    WHERE r.requestID = @RequestNumber;
END