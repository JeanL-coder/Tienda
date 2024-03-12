import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../services/product-service.service'; 
import { ApiService } from '../../services/api.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.css']
})
export class HomePageComponent implements OnInit {
  products: any[] = [];
  public users: any[] = [];

  constructor(private ProductService: ProductService, private api : ApiService, private auth : AuthService) { }

  ngOnInit(): void {
      this.api.getUsers()
      .subscribe(res=>{
        this.users = res; 
      })
    }
      logOut(){
        this.auth.signOut();
      }
    }
  


