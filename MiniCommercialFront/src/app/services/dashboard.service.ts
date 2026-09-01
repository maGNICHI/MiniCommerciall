import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

// Définition de l'interface pour typer la réponse du serveur
export interface DashboardStats {
  totalClients: number;
  totalOrders: number;
  totalRevenue: number;
  productsInStock: number;
}

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  // L'URL de votre API (ajustez le port si nécessaire)
  private apiUrl = 'https://localhost:7121/api/dashboard';

  constructor(private http: HttpClient) { }

  /**
   * Récupère les statistiques du tableau de bord
   * @param start Date de début au format YYYY-MM-DD (optionnel)
   * @param end Date de fin au format YYYY-MM-DD (optionnel)
   */
  getStats(start?: string, end?: string): Observable<DashboardStats> {
    let params = new HttpParams();

    // On n'ajoute les paramètres que s'ils ont une valeur
    if (start) {
      params = params.append('start', start);
    }
    if (end) {
      params = params.append('end', end);
    }

    return this.http.get<DashboardStats>(this.apiUrl, { params });
  }
}