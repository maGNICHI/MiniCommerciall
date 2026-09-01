import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ClientService } from '../../services/client.service';
import { OrderService } from 'src/app/services/order-service.service';
import { ProductService } from 'src/app/services/product-service.service';
@Component({
  selector: 'app-client-form',
  templateUrl: './client-form.component.html',
  styleUrls: ['./client-form.component.css']
})
export class ClientFormComponent implements OnInit {
  clientForm!: FormGroup;
  clientId: number | null = null;
  isEditMode = false;
  loading = false;

  constructor(
    private fb: FormBuilder,
    private clientService: ClientService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.createForm();
  }

  ngOnInit(): void {
    this.clientId = this.route.snapshot.params['id'];
    if (this.clientId) {
      this.isEditMode = true;
      this.loadClientData();
    }
  }

  createForm() {
    this.clientForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required, Validators.pattern('^[0-9+ ]*$')]],
      address: ['', [Validators.required, Validators.minLength(5)]]
    });
  }

  loadClientData() {
    this.loading = true;
    this.clientService.getClient(this.clientId!).subscribe({
      next: (client) => {
        this.clientForm.patchValue(client);
        this.loading = false;
      },
      error: (err) => {
        console.error(err);
        this.loading = false;
        this.router.navigate(['/clients']);
      }
    });
  }

  onSubmit() {
  if (this.clientForm.invalid) {
    this.clientForm.markAllAsTouched();
    return;
  }

  // On prépare les données de base
  const clientData: any = {
    name: this.clientForm.value.name,
    email: this.clientForm.value.email,
    phone: this.clientForm.value.phone,
    address: this.clientForm.value.address
  };

  this.loading = true;

  if (this.isEditMode) {
    // IMPORTANT : Pour une mise à jour (PUT), le backend attend souvent l'ID dans le JSON
    clientData.id = Number(this.clientId); 

    this.clientService.updateClient(this.clientId!, clientData).subscribe({
      next: () => {
        this.loading = false;
        this.router.navigate(['/clients']);
      },
      error: (err) => {
        this.loading = false;
        console.error("Détail de l'erreur 400 :", err.error); // Regardez ceci dans la console !
        alert("Erreur lors de la modification. Vérifiez la console.");
      }
    });
  } else {
    // Mode Création (POST) - En général, on n'envoie pas l'ID ici
    this.clientService.createClient(clientData).subscribe({
      next: () => {
        this.loading = false;
        this.router.navigate(['/clients']);
      },
      error: (err) => {
        this.loading = false;
        alert("Erreur lors de la création.");
      }
    });
  }
}
}