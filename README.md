# Морской Бой

![](https://sun9-2.userapi.com/impg/jaBpxXXEa7LTH6dOya9W-_rzIz5DqU0bP-fEvw/CY97xqBa4C0.jpg?size=688x450&quality=96&sign=3489bb44602b85a72be2705b24d914eb&type=album)

## Описание
**Третий проект из трёх по программированию на 3 семестре.**

## Структура проекта
| Класс  |Описание   |
| ------------ | ------------ |
| GameEngine.cs  | Определяет все строки, константы, цвета и пути проекта  |
| GameField.cs  |Реализует движок игры, полностью отделенный от графической части   |
|ISolver.cs| Интерфейс для всех типов искусственного интеллекта игры|
|AIPureRandom.cs| Реализует легкий режим игры (Тупой интеллект)|
|AIIntellectual.cs| Реализует сложный режим игры (Очень умный интеллект)|

|Окно|Описание|
| ------------ | ------------ |
|GameWindow|Главное окно игры, в него подгружается заранее сгенерированное поле игрока и противника|
|InitWindow|Главное меню игры с выбором сложности|
|ManualPlaceShipsWindow|Окно редактора игрового поля|
|PlayerWonWindow| Окно результата игры (выигрыш, проигрыш)|
|Tutorial| Туториал к игре|

## Описание
Главной целью было сделать так, чтобы движок игры и графика были как можно больше отделены друг от друга. Движок выделен в отдельный класс, общение к окном происходит через вызовы таких методов как ``GameField.RemoveShip``,  ``GameField.PlaceShip`` и т.д.
<div style="display: flex; justify-content: center;">
<img src="https://sun9-35.userapi.com/impg/zkeA4P4XRUeu3_hMrB711dVbidY7xbc-lMIieg/94JUA0jMlcU.jpg?size=739x645&quality=96&sign=99c6e916fa265ef895fc2ad4c6327ac7&type=album" width="500">
</div>
После обновления игрового поля, окно может (а может и нет) запросить у поля его состояние и уже само определить то, как оно будет отрисовано.
Благодаря такому разделению, экземпляр движка игры создается в окне редактора и передается как параметр в окно игры. В свою очередь, окно игры рисует в одном поле состояние поля игрока, в другом поле - состояние поля компьютера (далее Противника). Поле противника это такой-же экземпляр класса GameField, но со случайной генерацией кораблей.

## Искусственный - да,  интеллект - нет
Вся реализация работы ИИ строится на интерфейсе ``ISolver``.
В зависимости от сложности игры, фабрика ``AIFactory`` возвращает одну из реализаций ``ISolver``. Далее, в зависимости от хода игрока или противника вызывается или ``ISolver.MakeStep()``, или ``GameEngine.PerformAttack(Point p)``. Оба метода возвращают кортеж из ``int`` и ``List<Point>``, где первое значение отражает результат атаки, а второе - точки поля, которые эта атака затронула. Таким образом отрисовщику нужно будет опрашивать только затронутые клетки поля (и их соседей), а не всё поле. Экономия ресурсов. 

## Что можно было сделать лучше
После каждого хода отрисовщик вызывается вручную. Можно было в экземпляре поля вызывать событие его изменения, а в классе окна подписывать на него отрисовщик. Тогда больше не пришлось бы вообще больше думать о правильной отрисовке поля после каждого действия. 
