import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class ToastService {
  // Vous pouvez utiliser une bibliothèque comme 'ngx-toastr' 
  // ou simplement un BehaviorSubject pour votre propre composant alert.
  success(msg: string) { /* logique d'affichage */ console.log('SUCCESS:', msg); }
  error(msg: string) { /* logique d'affichage */ console.error('ERROR:', msg); }
}