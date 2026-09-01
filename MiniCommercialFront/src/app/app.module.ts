import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { OrderFormComponent } from './component/order-form/order-form.component';
import { OrderListComponent } from './component/order-list/order-list.component';
import { ProductListComponent } from './component/product-list/product-list.component';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ClientListComponent } from './component/client-list/client-list.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { ClientFormComponent } from './component/client-form/client-form.component';
import { ProductFormComponent } from './component/product-form/product-form.component';
import { OrderDetailComponent } from './component/order-detail/order-detail.component';
import { LoginComponent } from './component/login/login.component';
import { RegisterComponent } from './component/register/register.component';
import { JwtInterceptor } from './services/interpector/jwt.interceptor';
import { DashboardComponent } from './component/dashboard/dashboard.component';

@NgModule({
  declarations: [
    AppComponent,
    ClientListComponent,
    ClientFormComponent,
    OrderListComponent,
    OrderFormComponent,
    ProductListComponent,
    ProductFormComponent,
    OrderDetailComponent,
    LoginComponent,
    RegisterComponent,
    DashboardComponent
  ],
  imports: [
    BrowserModule,
    CommonModule,      // Important pour les pipes (date, currency)
    AppRoutingModule,  // Important pour routerLink et router-outlet
    HttpClientModule,  // Pour vos services
    ReactiveFormsModule, // Important pour [formGroup] et FormArray
    FormsModule
  ],
  providers: [{ provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true }],
  bootstrap: [AppComponent]
})
export class AppModule { }
