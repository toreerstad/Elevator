# Elevator

Man får instruksjoner om hvordan gi kommandoer ved oppstart av console-appen.
Heisens bevegelser simuleres vha delayed tasks, og bevegelsene logges til console.
Man får også logget estimert tid til etasjen man ønsker at heisen skal gå til.
Man kan angi nye destinasjoner mens heisen er i gang.

Det er mange begrensninger i programmet. Det skilles ikke på heistrykk innenfra og utenfor heisen. Utenfor heisen burde man kunne angi om man vil opp eller ned. Det er ikke implementert. Nødstopp er heller ikke implementert.
Når heisen er på vei oppover stopper den på alle destinasjoner oppover før den går til destinasjoner nedover, og omvent.
Nederste etasje er første, øverste etasje er default åttende. Kan endres i appsettings.json.

Det er et testprosjekt med to enkle tester.

Føler jeg har brukt nok tid på dette selv om det er mye som kan forbedres.
