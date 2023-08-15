import { Router, ActivatedRoute } from '@angular/router';
import { AppComponentBase } from '@shared/app-component-base';
import { Component, OnInit, Injector } from '@angular/core';
import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';

@Component({
  selector: 'app-available-resource-tab',
  templateUrl: './available-resource-tab.component.html',
  styleUrls: ['./available-resource-tab.component.css']
})
export class AvailableResourceTabComponent extends AppComponentBase implements OnInit {
  currentUrl: string = ""

  Resource_TabPool = PERMISSIONS_CONSTANT.Resource_TabPool
  Resource_TabAllResource = PERMISSIONS_CONSTANT.Resource_TabAllResource
  Resource_TabVendor = PERMISSIONS_CONSTANT.Resource_TabVendor
  constructor(injector: Injector, private router: Router, private route: ActivatedRoute) {
    super(injector);
    this.currentUrl =this.router.url
   
  }

  ngOnInit(): void {
    this.router.navigate(['pool'],{
      relativeTo:this.route,
      replaceUrl:true
    })
    this.router.events.subscribe(res => this.currentUrl = this.router.url)

  }

  routingPlanResourceTab(){
    this.router.navigate(['pool'],{
      relativeTo:this.route,
    })

  }
  routingFutureResourceTab(){
    this.router.navigate(['future-resource'],{
      relativeTo:this.route
    })
  }
  routingAllResourceTab(){
    this.router.navigate(['all-resource'],{
      relativeTo:this.route
    })
  }
  routingVendorTab(){
    this.router.navigate(['vendor'],{
      relativeTo:this.route
    })
  }
  reloadComponent() {
    this.router.navigateByUrl('', { skipLocationChange: true }).then(() => {
      this.router.navigate(['/app/accountType']);
    });
  }

  

}
