import { Component } from '@angular/core';
import { AuthService } from './services/auth-service.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  
  // L'injection doit être PUBLIC pour être utilisée dans le HTML (*ngIf="authService...")
  constructor(public authService: AuthService) {}

  // Getter pour récupérer dynamiquement le nom d'utilisateur stocké
  get username(): string {
    return this.authService.getUsername();
  }

  // Méthode de déconnexion appelée par le bouton (click)="onLogout()"
  onLogout() {
    this.authService.logout();
  }
}