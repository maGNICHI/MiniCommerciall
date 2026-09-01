import { Component, OnInit } from '@angular/core';
import { DashboardService } from 'src/app/services/dashboard.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
 stats: any;
  startDate: string = ''; // Initialisé comme chaîne vide
  endDate: string = '';


  constructor(private dashboardService: DashboardService) {}

  ngOnInit(): void {
    this.loadStats();
  }

  loadStats() {
    this.dashboardService.getStats(this.startDate, this.endDate).subscribe(res => {
      this.stats = res;
    });
  }

  onFilter() {
    this.loadStats();
  }
}
