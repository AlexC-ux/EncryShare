# EncryShare - безопасный канал связи

Устанавливаем или компилируем исполняемый файл, затем запускаем.

![image](https://user-images.githubusercontent.com/55258939/157244431-61fb481f-898c-47fe-aab5-36fe88db8b18.png)

На 1 машине нажимаем "WaitConnection"
На 2 машине нажимаем "Connect"


На 1 машине:

![image](https://user-images.githubusercontent.com/55258939/157244493-a50e4b08-ef06-4811-bf6b-8de32b933656.png)

В поле напротив кнопки "listen" можно ввести IP второй машины, затем нажимаем "listen"

![image](https://user-images.githubusercontent.com/55258939/157244538-3415df7d-42b6-4320-bcc4-b8d903eafd3a.png)


В окне 2 машины:

![image](https://user-images.githubusercontent.com/55258939/157244585-00d3f001-2c75-427c-ae73-0b2953363bcc.png)

В поле напротив кнопки "connect" вводим IP первой машины, затем нажимаем кнопку.

Далее произойдёт обмен ключами и канал связи можно считать налаженным.


P.S.Если на 1 машине не было введено IP второй машины, то нужно будет
подтвердить подключение незнакомого IP (Это будет ip второй машины)


# FAQ

## Какой IP указывать
IP адресс будет указан здесь:

![image](https://user-images.githubusercontent.com/55258939/157244773-5d40c215-4144-418d-914a-602eb2ea4592.png)

Если Вы используете VPN или Proxy, как и я, то
можете не переживать, в программе он так же изменится

## Не могу соединиться
Если Вы не можете соединиться со второй машиной, то проверьте FireWall, программе нужен доступ к 60766 и 60755 портам, так же эти порты должны быть проброшены на роутере.
