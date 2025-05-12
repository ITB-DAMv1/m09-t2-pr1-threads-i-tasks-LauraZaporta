## T2 PR1 Threads i Tasks:
--------------------------

### ENTREGABLE 1:

- **Explicació solució proposada: **Al primer entregable he seguit una estratègia que utilitza llistes d'espera pels palets que es desenvolupa de la següent manera: Cada comensal té dos objectes palets associats (l'esquerre i el dret) i aquests tenen
  com a propietat una llista d'espera que emmagatzema el número de comensals que volen el palet. Quan comença un cicle d'un fil el comensal pensa, s'afegeix a la llista d'espera dels seus palets i quan ambdós estan disponibles (el comensal està
  a la posició [0] de les dues llistes d'espera dels palets) menja. A l'acabar deixa els palets (s'elimina de les llistes dels palets) i torna a pensar. D'aquesta manera i afegint un bloqueig (lock) a l'acció d'afegir-se a la llista de cada comensal
  s'eviten interbloquejos òptimament i s'evita la mort per gana eficientment.
- **Dades d'anàlisi: **0 comensals han passat fam, cada comensal ha menjat 10 vegades de mitjana (límit establert per evitar bucle indefinit)...


### ENTREGABLE 2:

- **Explicació solució proposada: **Al segona entregable he fet ús de diverses funcions i Tasks separades per executar exitosament el joc; per un lloc la funció principal RunGame() que engloba totes les demés funcions i declara com es realitzen
  les tasks per tal d'executar al mateix temps el render, la lògica, l'input, etc., per altra la funció relativa al Render() que pinta continuament a la pantalla les dades variants de la nau i els asteroides a la freqüència establerta i finalment
  les funcions restants que consisteixen de la lògica (generació d'asteroides, moviment de la nau, col·lisió, generació d'sprites...). També he emprat un cancellation token per parar totes les Tasks quan l'usuari ho decideixi (Q) o quan s'acaba
  de carregar la web hipotètica. Sí que he diferenciat entre programació asíncrona i paral·lela ja que hi ha mètodes que utilizen async i await que eviten el bloquieg del bucle principal (asíncrona) i n'hi ha d'altres que simplement s'executen
  paral·lelament amb altres fils sense cap espera (paral·lela).


### Bibliografia:

- Anònim. (29/01/2019). C# | How to play Beep sound through Console. GeeksForGeeks. Recuperat 11/05/2025 de https://www.geeksforgeeks.org/c-sharp-how-to-play-beep-sound-through-console/
- Uday Dodiya. (Sense indicació de data). Converting Integers to Characters in C#. C# Corner. Recuperat 11/05/2025 de https://www.c-sharpcorner.com/article/converting-integers-to-characters-in-c-sharp/
- Documentació oficial de .NET - Busca how to clear console c#: https://learn.microsoft.com/es-es/dotnet/api/system.console.clear?view=net-9.0
- CodeCaster. (22/08/2014). Append lines to a file using a StreamWriter. StackOverflow. Recuperat 11/05/2025 de https://stackoverflow.com/questions/7306214/append-lines-to-a-file-using-a-streamwriter
- A chatGPT... Hola ChatGPT! Què és exactament Console.SetBufferSize(); i què fa?
  Hola! 😊

  Console.SetBufferSize(int width, int height) és una funció de la classe Console de .NET que estableix la mida del buffer de la consola, és a dir, la mida real de la zona de text que pot contenir la consola, més enllà del que veus a la finestra.
  
  🔍 Diferència entre Window Size i Buffer Size:
  Window Size (ex: Console.SetWindowSize(150, 40)) defineix la mida visible de la finestra de consola.
  
  Buffer Size (ex: Console.SetBufferSize(150, 100)) defineix la mida total de la zona de text, incloent les parts que pots fer scroll per veure.
  
  🧠 Exemple pràctic:
  Si tens això:
  
  csharp
  Copiar
  Editar
  Console.SetWindowSize(100, 30);
  Console.SetBufferSize(100, 100);
  Estàs dient que:
  
  La finestra mostra 30 línies a la vegada.
  
  Però el buffer conté 100 línies en total, així que pots fer scroll up i veure línies anteriors.
  
  ⚠️ Per què s’utilitza?
  En jocs o aplicacions amb moviment (com el teu amb asteroides 🚀), el BufferSize es fa coincidir amb el WindowSize per evitar el scroll automàtic, que causaria parpelleig o problemes visuals.
  
  Per exemple:
  
  csharp
  Copiar
  Editar
  Console.SetWindowSize(150, 100);
  Console.SetBufferSize(150, 100); // Evita que la consola faci scroll
