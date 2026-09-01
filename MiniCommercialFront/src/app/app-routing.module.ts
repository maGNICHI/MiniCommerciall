import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';


// Imports des Composants
import { LoginComponent } from './component/login/login.component';
import { RegisterComponent } from './component/register/register.component';
import { DashboardComponent } from './component/dashboard/dashboard.component';
import { ClientListComponent } from './component/client-list/client-list.component';
import { ClientFormComponent } from './component/client-form/client-form.component';
import { ProductListComponent } from './component/product-list/product-list.component';
import { ProductFormComponent } from './component/product-form/product-form.component';
import { OrderListComponent } from './component/order-list/order-list.component';
import { OrderFormComponent } from './component/order-form/order-form.component';
import { OrderDetailComponent } from './component/order-detail/order-detail.component';
import { authGuard } from './services/guard/auth.guard';

const routes: Routes = [
  // --- ROUTES PUBLIQUES ---
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },

  // --- ROUTES PROTÉGÉES (Nécessitent d'être connecté) ---
  
  // Accueil / Dashboard
  { path: 'dashboard', component: DashboardComponent, canActivate: [authGuard] },

  // Gestion des Clients
  { path: 'clients', component: ClientListComponent, canActivate: [authGuard] },
  { path: 'clients/new', component: ClientFormComponent, canActivate: [authGuard] },
  { path: 'clients/edit/:id', component: ClientFormComponent, canActivate: [authGuard] },

  // Gestion des Produits
  { path: 'products', component: ProductListComponent, canActivate: [authGuard] },
  { path: 'products/new', component: ProductFormComponent, canActivate: [authGuard] },
  { path: 'products/edit/:id', component: ProductFormComponent, canActivate: [authGuard] },

  // Gestion des Commandes
  { path: 'orders', component: OrderListComponent, canActivate: [authGuard] },
  { path: 'orders/new', component: OrderFormComponent, canActivate: [authGuard] },
  { path: 'orders/edit/:id', component: OrderFormComponent, canActivate: [authGuard] },
  { path: 'orders/:id', component: OrderDetailComponent, canActivate: [authGuard] },

  // --- REDIRECTIONS ET 404 ---
  
  // Par défaut, on redirige vers le Dashboard
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  
  // Si la route n'existe pas, retour au dashboard
  { path: '**', redirectTo: '/dashboard' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }