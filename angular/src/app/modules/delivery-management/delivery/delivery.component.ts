import { PERMISSIONS_CONSTANT } from './../../../constant/permission.constant';
import { Router, ActivatedRoute } from '@angular/router';
import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';

@Component({
  selector: 'app-delivery',
  templateUrl: './delivery.component.html',
  styleUrls: ['./delivery.component.css']
})
export class DeliveryComponent extends AppComponentBase implements OnInit {
  // DeliveryManagement_CanViewMenu_ResourceManagement=PERMISSIONS_CONSTANT.Deliverymanagement_CanViewMenu_ResourceManagement;
  // DeliveryManagement_CanViewMenu_WeeklyReport=PERMISSIONS_CONSTANT.DeliveryManagement_CanViewMenu_WeeklyReport;
  // DeliveryManagement_ResourceRequest_AvailableResource = PERMISSIONS_CONSTANT.DeliveryManagement_ResourceRequest_AvailableResource
  currentUrl: string = ""

  constructor(injector: Injector, private router: Router, private route: ActivatedRoute) {
    super(injector)
  }

  ngOnInit(): void {
    // this.currentUrl =this.router.url
    // this.router.events.subscribe(res => this.currentUrl = this.router.url)
    // this.router.navigate(['weekly-report-tab'], {
    //   relativeTo: this.route ,
    //   replaceUrl: true
    // })
  }

  
  // routingWeeklyReportTab(){
  //   this.router.navigate(['weekly-report-tab'],{
  //     relativeTo:this.route
  //   })
    
  // }
  // routingRequestResourceTab(){
  //   this.router.navigate(['request-resource-tab'],{
  //     relativeTo:this.route
  //   })
  // }
  // routingAvailableResourceTab(){
  //   this.router.navigate(['available-resource-tab/plan-resource'],{
  //     relativeTo:this.route
  //   })
  // }
}
