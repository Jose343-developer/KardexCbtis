using System;

namespace ML;

public class Grupo
{
public int? IdGrupo {get; set;}
public Int16? Semestre {get; set;}
public string? Letra {get; set;}
public string? Turno {get; set;}

public List<object> ?Turnos {get; set;}
public List<object> ?semestres {get; set;}

public List<object> ?grupos {get; set;}

}
