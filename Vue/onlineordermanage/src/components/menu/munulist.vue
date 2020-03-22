<template>
  <div>
    <el-container style="border: 1px solid #eee">
      <el-aside width="250px">
        <el-menu router class="el-menu-vertical-demo" :default-active="this.$router.path">
          <el-submenu v-for="(item,i) in navList" :key="i+1" v-show="item.second" index="item.name">
            <template slot="title">{{ item.navItem }}</template>
            <el-menu-item
              v-for="(sec,j) in item.second"
              :key="(i+1)-(j+1)"
              :index="sec.name"
            >{{ sec.navItem }}</el-menu-item>
          </el-submenu>
          <el-menu-item
            v-for="(item,i) in navList"
            :key="(i+1)"
            v-show="!item.second"
            :index="item.name"
          >{{ item.navItem }}</el-menu-item>
        </el-menu>
      </el-aside>
      <el-main>
        <router-view class="routerView"></router-view>
      </el-main>
    </el-container>
  </div>
</template>
<style>
.el-menu-vertical-demo:not(.el-menu--collapse) {
  min-width: 200px;
  min-height: 400px;
  height: 100%;
}
</style>

<script>
export default {
  data() {
    return {
      navList: [
        {
          name: "/prolist",
          navItem: "产品管理",
          second: [
            { name: "/procategorylist", navItem: "产品分类管理" },
            { name: "/prolist", navItem: "产品管理" }
          ]
        }
      ],
      isCollapse: true
    };
  },
  methods: {
    handleOpen(key, keyPath) {
      console.log(key, keyPath);
    },
    handleClose(key, keyPath) {
      console.log(key, keyPath);
    },
    handleSelect(key) {
      if (key == 0) {
        this.isCollapse = !this.isCollapse;
      }
      this.collapseText = this.isCollapse ? "展开" : "收起";
    }
  }
};
</script>

<style>
</style>