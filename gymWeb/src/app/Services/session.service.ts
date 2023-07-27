import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SessionService {

  constructor() { }

  guardarSession(idSesion: number){
    localStorage.setItem('gimnasio', JSON.stringify(idSesion));
  }

  obtenerSession(){
    const dataSession = localStorage.getItem('gimnasio');
    const gimnacio = JSON.parse(dataSession!);
    return gimnacio;
  }

  eliminarSession(){
    localStorage.removeItem('gimnasio')
  }
}