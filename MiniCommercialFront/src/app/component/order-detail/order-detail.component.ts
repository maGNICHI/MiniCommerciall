import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { OrderService } from 'src/app/services/order-service.service';

@Component({
  selector: 'app-order-detail',
  templateUrl: './order-detail.component.html',
  styleUrls: ['./order-detail.component.css']
})
export class OrderDetailComponent implements OnInit {
  order: any = null;
  loading = true;
  totalHT = 0;

  constructor(
    private route: ActivatedRoute,
    private orderService: OrderService,
    private router: Router
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.params['id'];
    if (id) {
      this.loadOrderDetail(id);
    }
  }

  loadOrderDetail(id: number) {
    this.loading = true;
    this.orderService.getOrder(id).subscribe({
      next: (data) => {
        this.order = data;
        this.calculateSubTotal();
        this.loading = false;
      },
      error: (err) => {
        console.error(err);
        this.loading = false;
        alert("Erreur lors du chargement des détails de la commande.");
      }
    });
  }

  calculateSubTotal() {
    if (this.order && this.order.lines) {
      this.totalHT = this.order.lines.reduce((acc: number, line: any) => 
        acc + (line.quantity * line.unitPrice), 0);
    }
  }

  printOrder() {
    window.print(); // Fonction native pour imprimer la page
  }

  goBack() {
    this.router.navigate(['/orders']);
  }
}