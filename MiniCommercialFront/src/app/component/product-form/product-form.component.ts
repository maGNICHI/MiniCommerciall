import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from 'src/app/services/product-service.service';

@Component({
  selector: 'app-product-form',
  templateUrl: './product-form.component.html',
  styleUrls: ['./product-form.component.css']
})
export class ProductFormComponent implements OnInit {
  productForm!: FormGroup;
  productId: number | null = null;
  isEditMode = false;
  
  // AJOUTEZ CETTE LIGNE ICI :
  loading = false; 

  constructor(
    private fb: FormBuilder,
    private productService: ProductService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.initForm();
  }

  ngOnInit(): void {
    this.productId = this.route.snapshot.params['id'];
    if (this.productId) {
      this.isEditMode = true;
      this.loadProductData();
    }
  }

  initForm() {
  this.productForm = this.fb.group({
    // Ajoutez la référence ici
    reference: ['', [Validators.required]], 
    name: ['', [Validators.required, Validators.minLength(2)]],
    description: [''],
    unitPriceHT: [0, [Validators.required, Validators.min(0.01)]],
    stockQuantity: [0, [Validators.required, Validators.min(0)]]
  });
}



  loadProductData() {
    this.productService.getProduct(this.productId!).subscribe(product => {
      this.productForm.patchValue(product);
    });
  }

  onSubmit() {
    if (this.productForm.invalid) {
      this.productForm.markAllAsTouched();
      return;
    }

    this.loading = true; // Maintenant cela fonctionnera

    const productData: any = {
      id: this.isEditMode ? Number(this.productId) : 0,
      name: this.productForm.value.name,
     reference: this.productForm.value.reference, 
      description: this.productForm.value.description,
      unitPriceHT: Number(this.productForm.value.unitPriceHT),
      stockQuantity: Number(this.productForm.value.stockQuantity)
    };

    if (this.isEditMode) {
      this.productService.updateProduct(this.productId!, productData).subscribe({
        next: () => {
          this.loading = false;
          this.router.navigate(['/products']);
        },
        error: (err) => {
          this.loading = false;
          console.error(err);
        }
      });
    } else {
      this.productService.createProduct(productData).subscribe({
        next: () => {
          this.loading = false;
          this.router.navigate(['/products']);
        },
        error: (err) => {
          this.loading = false;
          console.error(err);
        }
      });
    }
  }
}