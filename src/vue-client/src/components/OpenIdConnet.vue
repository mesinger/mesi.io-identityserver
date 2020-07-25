<template>
  <div>
    <button @click="login()">Login</button>
    <button @click="logout()">Logout</button>
    <button @click="api()">Api</button>
  </div>
</template>

<script lang="ts">
import Vue from 'vue'
import Component from 'vue-class-component';
import { OpenIdConnectService } from '../service/openid-connect';

@Component
export default class OpenIdConnect extends Vue{
  private readonly openId = new OpenIdConnectService();

  login() {
    this.openId.login();
  }

  logout() {
    this.openId.logout();
  }

  api() {
    this.openId.api();
  }
}
</script>
