<template>
  <div>
    <el-container style="border: 1px solid #eee">
      <el-aside width="250px">
        <el-menu router class="el-menu-vertical-demo" :default-active="this.$router.path">
          <template v-for="item in navList">
            <el-submenu v-if="item.second" :key="item.id" v-show="item.second" index="item.name">
              <template slot="title">{{ item.navItem }}</template>
              <el-menu-item
                v-for="(sec) in item.second"
                :key="sec.id"
                :index="sec.name"
              >{{ sec.navItem }}</el-menu-item>
            </el-submenu>
          </template>
          <template v-for="item in navList">
            <el-menu-item
              v-if="!item.second"
              v-show="!item.second"
              :key="item.id"
              :index="item.name"
            >{{ item.navItem }}</el-menu-item>
          </template>
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
          id: "1",
          name: "/prolist",
          navItem: "产品管理",
          second: [
            { id: 2, name: "/procategorylist", navItem: "产品分类管理" },
            { id: 3, name: "/prolist", navItem: "产品管理" }
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