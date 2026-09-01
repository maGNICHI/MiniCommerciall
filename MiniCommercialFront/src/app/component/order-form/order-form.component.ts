import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { OrderService } from 'src/app/services/order-service.service';
import { ClientService } from 'src/app/services/client.service';
import { ProductService } from 'src/app/services/product-service.service';

@Component({
  selector: 'app-order-form',
  templateUrl: './order-form.component.html',
  styleUrls: ['./order-form.component.css']
})
export class OrderFormComponent implements OnInit {
  orderForm!: FormGroup;
  clients: any[] = [];
  products: any[] = [];
  isEditMode = false;
  orderId: number | null = null;
  loading = false;
  
  totalHT = 0;
  totalTTC = 0;
  private readonly TVA_TAUX = 0.19; // 19%

  constructor(
    private fb: FormBuilder,
    private orderService: OrderService,
    private clientService: ClientService,
    private productService: ProductService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.initForm();
  }

  ngOnInit(): void {
    this.loading = true;

    // 1. Charger d'abord les listes de base (Clients et Produits)
    this.clientService.getClients().subscribe(clients => {
      this.clients = clients;
      
      this.productService.getProducts().subscribe(products => {
        this.products = products;

        // 2. Vérifier l'ID dans l'URL
        const idParam = this.route.snapshot.params['id'];
        
        if (idParam && idParam !== 'new') {
          this.orderId = Number(idParam);
          this.isEditMode = true;
          this.loadOrderForEdit();
        } else {
          this.isEditMode = false;
          this.addLine();
          this.loading = false;
        }
      });
    });
  }

  initForm() {
    this.orderForm = this.fb.group({
      clientId: ['', Validators.required],
      status: ['Brouillon'], 
      lines: this.fb.array([]) 
    });
  }

  get lines() {
    return this.orderForm.get('lines') as FormArray;
  }

  addLine() {
    const line = this.fb.group({
      productId: ['', Validators.required],
      quantity: [1, [Validators.required, Validators.min(1)]],
      unitPrice: [0],
      totalLine: [0]
    });
    this.lines.push(line);
  }

  removeLine(index: number) {
    this.lines.removeAt(index);
    this.calculateTotals();
  }

  loadOrderForEdit() {
    if (!this.orderId) return;

    this.orderService.getOrder(this.orderId).subscribe({
      next: (order: any) => {
        this.orderForm.patchValue({
          clientId: order.clientId, 
          status: order.status
        });

        this.lines.clear();
        if (order.lines) {
          order.lines.forEach((l: any) => {
            this.lines.push(this.fb.group({
              productId: [l.productId, Validators.required],
              quantity: [l.quantity, [Validators.required, Validators.min(1)]],
              unitPrice: [l.unitPrice],
              totalLine: [l.quantity * l.unitPrice]
            }));
          });
        }

        this.calculateTotals();
        this.loading = false;
      },
      error: (err) => {
        console.error("Erreur lors du chargement :", err);
        this.loading = false;
        this.router.navigate(['/orders']);
      }
    });
  }

  onProductChange(index: number) {
    const line = this.lines.at(index);
    const selectedProd = this.products.find(p => p.id == line.value.productId);
    
    if (selectedProd) {
      line.patchValue({ unitPrice: selectedProd.unitPriceHT });
      this.calculateTotals();
    }
  }

  calculateTotals() {
    this.totalHT = 0;
    this.lines.controls.forEach(line => {
      const subTotal = line.value.quantity * line.value.unitPrice;
      line.patchValue({ totalLine: subTotal }, { emitEvent: false });
      this.totalHT += subTotal;
    });
    this.totalTTC = this.totalHT * (1 + this.TVA_TAUX);
  }

  submit() {
    if (this.orderForm.invalid) {
      this.orderForm.markAllAsTouched();
      return;
    }

    const rawValues = this.orderForm.value;

    // Construction du DTO propre
    const orderPayload = {
      clientId: Number(rawValues.clientId), 
      status: rawValues.status,
      lines: rawValues.lines.map((l: any) => ({
        productId: Number(l.productId),
        quantity: l.quantity,
        unitPrice: l.unitPrice
      }))
    };

    this.loading = true;

    if (this.isEditMode && this.orderId) {
      // MODE MISE À JOUR (PUT)
      this.orderService.updateOrder(this.orderId, orderPayload).subscribe({
        next: () => {
          this.loading = false;
          this.router.navigate(['/orders']);
        },
        error: (err) => {
          this.loading = false;
          console.error("Erreur PUT :", err);
          alert("Erreur lors de la modification : " + (err.error?.message || "Vérifiez les données"));
        }
      });
    } else {
      // MODE CRÉATION (POST)
      this.orderService.createOrder(orderPayload).subscribe({
        next: () => {
          this.loading = false;
          this.router.navigate(['/orders']);
        },
        error: (err) => {
          this.loading = false;
          console.error("Erreur POST :", err);
          alert("Erreur lors de la création : " + (err.error || "Vérifiez le stock"));
        }
      });
    }
  }
}