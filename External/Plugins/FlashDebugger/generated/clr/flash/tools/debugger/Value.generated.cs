//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by jni4net. See http://jni4net.sourceforge.net/ 
//     Runtime Version:2.0.50727.5472
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace flash.tools.debugger {
    
    
    #region Component Designer generated code 
    [global::net.sf.jni4net.attributes.JavaInterfaceAttribute()]
    public partial interface Value {
        
        [global::net.sf.jni4net.attributes.JavaMethodAttribute("()J")]
        long getId();
        
        [global::net.sf.jni4net.attributes.JavaMethodAttribute("()I")]
        int getType();
        
        [global::net.sf.jni4net.attributes.JavaMethodAttribute("()Ljava/lang/String;")]
        global::java.lang.String getTypeName();
        
        [global::net.sf.jni4net.attributes.JavaMethodAttribute("()Ljava/lang/String;")]
        global::java.lang.String getClassName();
        
        [global::net.sf.jni4net.attributes.JavaMethodAttribute("()I")]
        int getAttributes();
        
        [global::net.sf.jni4net.attributes.JavaMethodAttribute("(Lflash/tools/debugger/Session;)I")]
        int getMemberCount(global::flash.tools.debugger.Session par0);
        
        [global::net.sf.jni4net.attributes.JavaMethodAttribute("()I")]
        int getIsolateId();
        
        [global::net.sf.jni4net.attributes.JavaMethodAttribute("(I)Z")]
        bool isAttributeSet(int par0);
        
        [global::net.sf.jni4net.attributes.JavaMethodAttribute("()Ljava/lang/Object;")]
        global::java.lang.Object getValueAsObject();
        
        [global::net.sf.jni4net.attributes.JavaMethodAttribute("()Ljava/lang/String;")]
        global::java.lang.String getValueAsString();
        
        [global::net.sf.jni4net.attributes.JavaMethodAttribute("(Lflash/tools/debugger/Session;)[Lflash/tools/debugger/Variable;")]
        flash.tools.debugger.Variable[] getMembers(global::flash.tools.debugger.Session par0);
        
        [global::net.sf.jni4net.attributes.JavaMethodAttribute("(Lflash/tools/debugger/Session;Ljava/lang/String;)Lflash/tools/debugger/Variable;" +
            "")]
        global::flash.tools.debugger.Variable getMemberNamed(global::flash.tools.debugger.Session par0, global::java.lang.String par1);
        
        [global::net.sf.jni4net.attributes.JavaMethodAttribute("(Z)[Ljava/lang/String;")]
        java.lang.String[] getClassHierarchy(bool par0);
        
        [global::net.sf.jni4net.attributes.JavaMethodAttribute("()[Lflash/tools/debugger/Variable;")]
        flash.tools.debugger.Variable[] getPrivateInheritedMembers();
        
        [global::net.sf.jni4net.attributes.JavaMethodAttribute("(Ljava/lang/String;)[Lflash/tools/debugger/Variable;")]
        flash.tools.debugger.Variable[] getPrivateInheritedMemberNamed(global::java.lang.String par0);
    }
    #endregion
    
    #region Component Designer generated code 
    public partial class Value_ {
        
        public static global::java.lang.Class _class {
            get {
                return global::flash.tools.debugger.@__Value.staticClass;
            }
        }
        
        [global::net.sf.jni4net.attributes.JavaMethodAttribute("Ljava/lang/Object;")]
        public static global::java.lang.Object UNDEFINED {
            get {
                global::net.sf.jni4net.jni.JNIEnv @__env = global::net.sf.jni4net.jni.JNIEnv.ThreadEnv;
            using(new global::net.sf.jni4net.jni.LocalFrame(@__env, 10)){
                return global::net.sf.jni4net.utils.Convertor.FullJ2C<global::java.lang.Object>(@__env, @__env.GetStaticObjectFieldPtr(global::flash.tools.debugger.@__Value.staticClass, global::flash.tools.debugger.@__Value._UNDEFINED15));
            }
            }
        }
        
        [global::net.sf.jni4net.attributes.JavaMethodAttribute("J")]
        public static long UNKNOWN_ID {
            get {
                global::net.sf.jni4net.jni.JNIEnv @__env = global::net.sf.jni4net.jni.JNIEnv.ThreadEnv;
            using(new global::net.sf.jni4net.jni.LocalFrame(@__env, 10)){
                return ((long)(@__env.GetStaticLongField(global::flash.tools.debugger.@__Value.staticClass, global::flash.tools.debugger.@__Value._UNKNOWN_ID16)));
            }
            }
        }
        
        [global::net.sf.jni4net.attributes.JavaMethodAttribute("J")]
        public static long GLOBAL_ID {
            get {
                global::net.sf.jni4net.jni.JNIEnv @__env = global::net.sf.jni4net.jni.JNIEnv.ThreadEnv;
            using(new global::net.sf.jni4net.jni.LocalFrame(@__env, 10)){
                return ((long)(@__env.GetStaticLongField(global::flash.tools.debugger.@__Value.staticClass, global::flash.tools.debugger.@__Value._GLOBAL_ID17)));
            }
            }
        }
        
        [global::net.sf.jni4net.attributes.JavaMethodAttribute("J")]
        public static long THIS_ID {
            get {
                global::net.sf.jni4net.jni.JNIEnv @__env = global::net.sf.jni4net.jni.JNIEnv.ThreadEnv;
            using(new global::net.sf.jni4net.jni.LocalFrame(@__env, 10)){
                return ((long)(@__env.GetStaticLongField(global::flash.tools.debugger.@__Value.staticClass, global::flash.tools.debugger.@__Value._THIS_ID18)));
            }
            }
        }
        
        [global::net.sf.jni4net.attributes.JavaMethodAttribute("J")]
        public static long ROOT_ID {
            get {
                global::net.sf.jni4net.jni.JNIEnv @__env = global::net.sf.jni4net.jni.JNIEnv.ThreadEnv;
            using(new global::net.sf.jni4net.jni.LocalFrame(@__env, 10)){
                return ((long)(@__env.GetStaticLongField(global::flash.tools.debugger.@__Value.staticClass, global::flash.tools.debugger.@__Value._ROOT_ID19)));
            }
            }
        }
        
        [global::net.sf.jni4net.attributes.JavaMethodAttribute("J")]
        public static long BASE_ID {
            get {
                global::net.sf.jni4net.jni.JNIEnv @__env = global::net.sf.jni4net.jni.JNIEnv.ThreadEnv;
            using(new global::net.sf.jni4net.jni.LocalFrame(@__env, 10)){
                return ((long)(@__env.GetStaticLongField(global::flash.tools.debugger.@__Value.staticClass, global::flash.tools.debugger.@__Value._BASE_ID20)));
            }
            }
        }
        
        [global::net.sf.jni4net.attributes.JavaMethodAttribute("J")]
        public static long LEVEL_ID {
            get {
                global::net.sf.jni4net.jni.JNIEnv @__env = global::net.sf.jni4net.jni.JNIEnv.ThreadEnv;
            using(new global::net.sf.jni4net.jni.LocalFrame(@__env, 10)){
                return ((long)(@__env.GetStaticLongField(global::flash.tools.debugger.@__Value.staticClass, global::flash.tools.debugger.@__Value._LEVEL_ID21)));
            }
            }
        }
        
        [global::net.sf.jni4net.attributes.JavaMethodAttribute("Ljava/lang/String;")]
        public static global::java.lang.String TRAITS_TYPE_NAME {
            get {
                global::net.sf.jni4net.jni.JNIEnv @__env = global::net.sf.jni4net.jni.JNIEnv.ThreadEnv;
            using(new global::net.sf.jni4net.jni.LocalFrame(@__env, 10)){
                return global::net.sf.jni4net.utils.Convertor.StrongJ2CpString(@__env, @__env.GetStaticObjectFieldPtr(global::flash.tools.debugger.@__Value.staticClass, global::flash.tools.debugger.@__Value._TRAITS_TYPE_NAME22));
            }
            }
        }
    }
    #endregion
    
    #region Component Designer generated code 
    [global::net.sf.jni4net.attributes.JavaProxyAttribute(typeof(global::flash.tools.debugger.Value), typeof(global::flash.tools.debugger.Value_))]
    [global::net.sf.jni4net.attributes.ClrWrapperAttribute(typeof(global::flash.tools.debugger.Value), typeof(global::flash.tools.debugger.Value_))]
    internal sealed partial class @__Value : global::java.lang.Object, global::flash.tools.debugger.Value {
        
        internal new static global::java.lang.Class staticClass;
        
        internal static global::net.sf.jni4net.jni.MethodId _getId0;
        
        internal static global::net.sf.jni4net.jni.MethodId _getType1;
        
        internal static global::net.sf.jni4net.jni.MethodId _getTypeName2;
        
        internal static global::net.sf.jni4net.jni.MethodId _getClassName3;
        
        internal static global::net.sf.jni4net.jni.MethodId _getAttributes4;
        
        internal static global::net.sf.jni4net.jni.MethodId _getMemberCount5;
        
        internal static global::net.sf.jni4net.jni.MethodId _getIsolateId6;
        
        internal static global::net.sf.jni4net.jni.MethodId _isAttributeSet7;
        
        internal static global::net.sf.jni4net.jni.MethodId _getValueAsObject8;
        
        internal static global::net.sf.jni4net.jni.MethodId _getValueAsString9;
        
        internal static global::net.sf.jni4net.jni.MethodId _getMembers10;
        
        internal static global::net.sf.jni4net.jni.MethodId _getMemberNamed11;
        
        internal static global::net.sf.jni4net.jni.MethodId _getClassHierarchy12;
        
        internal static global::net.sf.jni4net.jni.MethodId _getPrivateInheritedMembers13;
        
        internal static global::net.sf.jni4net.jni.MethodId _getPrivateInheritedMemberNamed14;
        
        internal static global::net.sf.jni4net.jni.FieldId _UNDEFINED15;
        
        internal static global::net.sf.jni4net.jni.FieldId _UNKNOWN_ID16;
        
        internal static global::net.sf.jni4net.jni.FieldId _GLOBAL_ID17;
        
        internal static global::net.sf.jni4net.jni.FieldId _THIS_ID18;
        
        internal static global::net.sf.jni4net.jni.FieldId _ROOT_ID19;
        
        internal static global::net.sf.jni4net.jni.FieldId _BASE_ID20;
        
        internal static global::net.sf.jni4net.jni.FieldId _LEVEL_ID21;
        
        internal static global::net.sf.jni4net.jni.FieldId _TRAITS_TYPE_NAME22;
        
        private @__Value(global::net.sf.jni4net.jni.JNIEnv @__env) : 
                base(@__env) {
        }
        
        private static void InitJNI(global::net.sf.jni4net.jni.JNIEnv @__env, java.lang.Class @__class) {
            global::flash.tools.debugger.@__Value.staticClass = @__class;
            global::flash.tools.debugger.@__Value._getId0 = @__env.GetMethodID(global::flash.tools.debugger.@__Value.staticClass, "getId", "()J");
            global::flash.tools.debugger.@__Value._getType1 = @__env.GetMethodID(global::flash.tools.debugger.@__Value.staticClass, "getType", "()I");
            global::flash.tools.debugger.@__Value._getTypeName2 = @__env.GetMethodID(global::flash.tools.debugger.@__Value.staticClass, "getTypeName", "()Ljava/lang/String;");
            global::flash.tools.debugger.@__Value._getClassName3 = @__env.GetMethodID(global::flash.tools.debugger.@__Value.staticClass, "getClassName", "()Ljava/lang/String;");
            global::flash.tools.debugger.@__Value._getAttributes4 = @__env.GetMethodID(global::flash.tools.debugger.@__Value.staticClass, "getAttributes", "()I");
            global::flash.tools.debugger.@__Value._getMemberCount5 = @__env.GetMethodID(global::flash.tools.debugger.@__Value.staticClass, "getMemberCount", "(Lflash/tools/debugger/Session;)I");
            global::flash.tools.debugger.@__Value._getIsolateId6 = @__env.GetMethodID(global::flash.tools.debugger.@__Value.staticClass, "getIsolateId", "()I");
            global::flash.tools.debugger.@__Value._isAttributeSet7 = @__env.GetMethodID(global::flash.tools.debugger.@__Value.staticClass, "isAttributeSet", "(I)Z");
            global::flash.tools.debugger.@__Value._getValueAsObject8 = @__env.GetMethodID(global::flash.tools.debugger.@__Value.staticClass, "getValueAsObject", "()Ljava/lang/Object;");
            global::flash.tools.debugger.@__Value._getValueAsString9 = @__env.GetMethodID(global::flash.tools.debugger.@__Value.staticClass, "getValueAsString", "()Ljava/lang/String;");
            global::flash.tools.debugger.@__Value._getMembers10 = @__env.GetMethodID(global::flash.tools.debugger.@__Value.staticClass, "getMembers", "(Lflash/tools/debugger/Session;)[Lflash/tools/debugger/Variable;");
            global::flash.tools.debugger.@__Value._getMemberNamed11 = @__env.GetMethodID(global::flash.tools.debugger.@__Value.staticClass, "getMemberNamed", "(Lflash/tools/debugger/Session;Ljava/lang/String;)Lflash/tools/debugger/Variable;" +
                    "");
            global::flash.tools.debugger.@__Value._getClassHierarchy12 = @__env.GetMethodID(global::flash.tools.debugger.@__Value.staticClass, "getClassHierarchy", "(Z)[Ljava/lang/String;");
            global::flash.tools.debugger.@__Value._getPrivateInheritedMembers13 = @__env.GetMethodID(global::flash.tools.debugger.@__Value.staticClass, "getPrivateInheritedMembers", "()[Lflash/tools/debugger/Variable;");
            global::flash.tools.debugger.@__Value._getPrivateInheritedMemberNamed14 = @__env.GetMethodID(global::flash.tools.debugger.@__Value.staticClass, "getPrivateInheritedMemberNamed", "(Ljava/lang/String;)[Lflash/tools/debugger/Variable;");
            global::flash.tools.debugger.@__Value._UNDEFINED15 = @__env.GetStaticFieldID(global::flash.tools.debugger.@__Value.staticClass, "UNDEFINED", "Ljava/lang/Object;");
            global::flash.tools.debugger.@__Value._UNKNOWN_ID16 = @__env.GetStaticFieldID(global::flash.tools.debugger.@__Value.staticClass, "UNKNOWN_ID", "J");
            global::flash.tools.debugger.@__Value._GLOBAL_ID17 = @__env.GetStaticFieldID(global::flash.tools.debugger.@__Value.staticClass, "GLOBAL_ID", "J");
            global::flash.tools.debugger.@__Value._THIS_ID18 = @__env.GetStaticFieldID(global::flash.tools.debugger.@__Value.staticClass, "THIS_ID", "J");
            global::flash.tools.debugger.@__Value._ROOT_ID19 = @__env.GetStaticFieldID(global::flash.tools.debugger.@__Value.staticClass, "ROOT_ID", "J");
            global::flash.tools.debugger.@__Value._BASE_ID20 = @__env.GetStaticFieldID(global::flash.tools.debugger.@__Value.staticClass, "BASE_ID", "J");
            global::flash.tools.debugger.@__Value._LEVEL_ID21 = @__env.GetStaticFieldID(global::flash.tools.debugger.@__Value.staticClass, "LEVEL_ID", "J");
            global::flash.tools.debugger.@__Value._TRAITS_TYPE_NAME22 = @__env.GetStaticFieldID(global::flash.tools.debugger.@__Value.staticClass, "TRAITS_TYPE_NAME", "Ljava/lang/String;");
        }
        
        public long getId() {
            global::net.sf.jni4net.jni.JNIEnv @__env = this.Env;
            using(new global::net.sf.jni4net.jni.LocalFrame(@__env, 10)){
            return ((long)(@__env.CallLongMethod(this, global::flash.tools.debugger.@__Value._getId0)));
            }
        }
        
        public int getType() {
            global::net.sf.jni4net.jni.JNIEnv @__env = this.Env;
            using(new global::net.sf.jni4net.jni.LocalFrame(@__env, 10)){
            return ((int)(@__env.CallIntMethod(this, global::flash.tools.debugger.@__Value._getType1)));
            }
        }
        
        public global::java.lang.String getTypeName() {
            global::net.sf.jni4net.jni.JNIEnv @__env = this.Env;
            using(new global::net.sf.jni4net.jni.LocalFrame(@__env, 10)){
            return global::net.sf.jni4net.utils.Convertor.StrongJ2CpString(@__env, @__env.CallObjectMethodPtr(this, global::flash.tools.debugger.@__Value._getTypeName2));
            }
        }
        
        public global::java.lang.String getClassName() {
            global::net.sf.jni4net.jni.JNIEnv @__env = this.Env;
            using(new global::net.sf.jni4net.jni.LocalFrame(@__env, 10)){
            return global::net.sf.jni4net.utils.Convertor.StrongJ2CpString(@__env, @__env.CallObjectMethodPtr(this, global::flash.tools.debugger.@__Value._getClassName3));
            }
        }
        
        public int getAttributes() {
            global::net.sf.jni4net.jni.JNIEnv @__env = this.Env;
            using(new global::net.sf.jni4net.jni.LocalFrame(@__env, 10)){
            return ((int)(@__env.CallIntMethod(this, global::flash.tools.debugger.@__Value._getAttributes4)));
            }
        }
        
        public int getMemberCount(global::flash.tools.debugger.Session par0) {
            global::net.sf.jni4net.jni.JNIEnv @__env = this.Env;
            using(new global::net.sf.jni4net.jni.LocalFrame(@__env, 12)){
            return ((int)(@__env.CallIntMethod(this, global::flash.tools.debugger.@__Value._getMemberCount5, global::net.sf.jni4net.utils.Convertor.ParFullC2J<global::flash.tools.debugger.Session>(@__env, par0))));
            }
        }
        
        public int getIsolateId() {
            global::net.sf.jni4net.jni.JNIEnv @__env = this.Env;
            using(new global::net.sf.jni4net.jni.LocalFrame(@__env, 10)){
            return ((int)(@__env.CallIntMethod(this, global::flash.tools.debugger.@__Value._getIsolateId6)));
            }
        }
        
        public bool isAttributeSet(int par0) {
            global::net.sf.jni4net.jni.JNIEnv @__env = this.Env;
            using(new global::net.sf.jni4net.jni.LocalFrame(@__env, 12)){
            return ((bool)(@__env.CallBooleanMethod(this, global::flash.tools.debugger.@__Value._isAttributeSet7, global::net.sf.jni4net.utils.Convertor.ParPrimC2J(par0))));
            }
        }
        
        public global::java.lang.Object getValueAsObject() {
            global::net.sf.jni4net.jni.JNIEnv @__env = this.Env;
            using(new global::net.sf.jni4net.jni.LocalFrame(@__env, 10)){
            return global::net.sf.jni4net.utils.Convertor.FullJ2C<global::java.lang.Object>(@__env, @__env.CallObjectMethodPtr(this, global::flash.tools.debugger.@__Value._getValueAsObject8));
            }
        }
        
        public global::java.lang.String getValueAsString() {
            global::net.sf.jni4net.jni.JNIEnv @__env = this.Env;
            using(new global::net.sf.jni4net.jni.LocalFrame(@__env, 10)){
            return global::net.sf.jni4net.utils.Convertor.StrongJ2CpString(@__env, @__env.CallObjectMethodPtr(this, global::flash.tools.debugger.@__Value._getValueAsString9));
            }
        }
        
        public flash.tools.debugger.Variable[] getMembers(global::flash.tools.debugger.Session par0) {
            global::net.sf.jni4net.jni.JNIEnv @__env = this.Env;
            using(new global::net.sf.jni4net.jni.LocalFrame(@__env, 12)){
            return global::net.sf.jni4net.utils.Convertor.ArrayFullJ2C<flash.tools.debugger.Variable[], global::flash.tools.debugger.Variable>(@__env, @__env.CallObjectMethodPtr(this, global::flash.tools.debugger.@__Value._getMembers10, global::net.sf.jni4net.utils.Convertor.ParFullC2J<global::flash.tools.debugger.Session>(@__env, par0)));
            }
        }
        
        public global::flash.tools.debugger.Variable getMemberNamed(global::flash.tools.debugger.Session par0, global::java.lang.String par1) {
            global::net.sf.jni4net.jni.JNIEnv @__env = this.Env;
            using(new global::net.sf.jni4net.jni.LocalFrame(@__env, 14)){
            return global::net.sf.jni4net.utils.Convertor.FullJ2C<global::flash.tools.debugger.Variable>(@__env, @__env.CallObjectMethodPtr(this, global::flash.tools.debugger.@__Value._getMemberNamed11, global::net.sf.jni4net.utils.Convertor.ParFullC2J<global::flash.tools.debugger.Session>(@__env, par0), global::net.sf.jni4net.utils.Convertor.ParStrongCp2J(par1)));
            }
        }
        
        public java.lang.String[] getClassHierarchy(bool par0) {
            global::net.sf.jni4net.jni.JNIEnv @__env = this.Env;
            using(new global::net.sf.jni4net.jni.LocalFrame(@__env, 12)){
            return global::net.sf.jni4net.utils.Convertor.ArrayStrongJ2CpString(@__env, @__env.CallObjectMethodPtr(this, global::flash.tools.debugger.@__Value._getClassHierarchy12, global::net.sf.jni4net.utils.Convertor.ParPrimC2J(par0)));
            }
        }
        
        public flash.tools.debugger.Variable[] getPrivateInheritedMembers() {
            global::net.sf.jni4net.jni.JNIEnv @__env = this.Env;
            using(new global::net.sf.jni4net.jni.LocalFrame(@__env, 10)){
            return global::net.sf.jni4net.utils.Convertor.ArrayFullJ2C<flash.tools.debugger.Variable[], global::flash.tools.debugger.Variable>(@__env, @__env.CallObjectMethodPtr(this, global::flash.tools.debugger.@__Value._getPrivateInheritedMembers13));
            }
        }
        
        public flash.tools.debugger.Variable[] getPrivateInheritedMemberNamed(global::java.lang.String par0) {
            global::net.sf.jni4net.jni.JNIEnv @__env = this.Env;
            using(new global::net.sf.jni4net.jni.LocalFrame(@__env, 12)){
            return global::net.sf.jni4net.utils.Convertor.ArrayFullJ2C<flash.tools.debugger.Variable[], global::flash.tools.debugger.Variable>(@__env, @__env.CallObjectMethodPtr(this, global::flash.tools.debugger.@__Value._getPrivateInheritedMemberNamed14, global::net.sf.jni4net.utils.Convertor.ParStrongCp2J(par0)));
            }
        }
        
        private static global::System.Collections.Generic.List<global::net.sf.jni4net.jni.JNINativeMethod> @__Init(global::net.sf.jni4net.jni.JNIEnv @__env, global::java.lang.Class @__class) {
            global::System.Type @__type = typeof(__Value);
            global::System.Collections.Generic.List<global::net.sf.jni4net.jni.JNINativeMethod> methods = new global::System.Collections.Generic.List<global::net.sf.jni4net.jni.JNINativeMethod>();
            methods.Add(global::net.sf.jni4net.jni.JNINativeMethod.Create(@__type, "getId", "getId0", "()J"));
            methods.Add(global::net.sf.jni4net.jni.JNINativeMethod.Create(@__type, "getType", "getType1", "()I"));
            methods.Add(global::net.sf.jni4net.jni.JNINativeMethod.Create(@__type, "getTypeName", "getTypeName2", "()Ljava/lang/String;"));
            methods.Add(global::net.sf.jni4net.jni.JNINativeMethod.Create(@__type, "getClassName", "getClassName3", "()Ljava/lang/String;"));
            methods.Add(global::net.sf.jni4net.jni.JNINativeMethod.Create(@__type, "getAttributes", "getAttributes4", "()I"));
            methods.Add(global::net.sf.jni4net.jni.JNINativeMethod.Create(@__type, "getMemberCount", "getMemberCount5", "(Lflash/tools/debugger/Session;)I"));
            methods.Add(global::net.sf.jni4net.jni.JNINativeMethod.Create(@__type, "getIsolateId", "getIsolateId6", "()I"));
            methods.Add(global::net.sf.jni4net.jni.JNINativeMethod.Create(@__type, "isAttributeSet", "isAttributeSet7", "(I)Z"));
            methods.Add(global::net.sf.jni4net.jni.JNINativeMethod.Create(@__type, "getValueAsObject", "getValueAsObject8", "()Ljava/lang/Object;"));
            methods.Add(global::net.sf.jni4net.jni.JNINativeMethod.Create(@__type, "getValueAsString", "getValueAsString9", "()Ljava/lang/String;"));
            methods.Add(global::net.sf.jni4net.jni.JNINativeMethod.Create(@__type, "getMembers", "getMembers10", "(Lflash/tools/debugger/Session;)[Lflash/tools/debugger/Variable;"));
            methods.Add(global::net.sf.jni4net.jni.JNINativeMethod.Create(@__type, "getMemberNamed", "getMemberNamed11", "(Lflash/tools/debugger/Session;Ljava/lang/String;)Lflash/tools/debugger/Variable;" +
                        ""));
            methods.Add(global::net.sf.jni4net.jni.JNINativeMethod.Create(@__type, "getClassHierarchy", "getClassHierarchy12", "(Z)[Ljava/lang/String;"));
            methods.Add(global::net.sf.jni4net.jni.JNINativeMethod.Create(@__type, "getPrivateInheritedMembers", "getPrivateInheritedMembers13", "()[Lflash/tools/debugger/Variable;"));
            methods.Add(global::net.sf.jni4net.jni.JNINativeMethod.Create(@__type, "getPrivateInheritedMemberNamed", "getPrivateInheritedMemberNamed14", "(Ljava/lang/String;)[Lflash/tools/debugger/Variable;"));
            return methods;
        }
        
        private static long getId0(global::System.IntPtr @__envp, global::net.sf.jni4net.utils.JniLocalHandle @__obj) {
            // ()J
            // ()J
            global::net.sf.jni4net.jni.JNIEnv @__env = global::net.sf.jni4net.jni.JNIEnv.Wrap(@__envp);
            long @__return = default(long);
            try {
            global::flash.tools.debugger.Value @__real = global::net.sf.jni4net.utils.Convertor.FullJ2C<global::flash.tools.debugger.Value>(@__env, @__obj);
            @__return = ((long)(@__real.getId()));
            }catch (global::System.Exception __ex){@__env.ThrowExisting(__ex);}
            return @__return;
        }
        
        private static int getType1(global::System.IntPtr @__envp, global::net.sf.jni4net.utils.JniLocalHandle @__obj) {
            // ()I
            // ()I
            global::net.sf.jni4net.jni.JNIEnv @__env = global::net.sf.jni4net.jni.JNIEnv.Wrap(@__envp);
            int @__return = default(int);
            try {
            global::flash.tools.debugger.Value @__real = global::net.sf.jni4net.utils.Convertor.FullJ2C<global::flash.tools.debugger.Value>(@__env, @__obj);
            @__return = ((int)(@__real.getType()));
            }catch (global::System.Exception __ex){@__env.ThrowExisting(__ex);}
            return @__return;
        }
        
        private static global::net.sf.jni4net.utils.JniHandle getTypeName2(global::System.IntPtr @__envp, global::net.sf.jni4net.utils.JniLocalHandle @__obj) {
            // ()Ljava/lang/String;
            // ()Ljava/lang/String;
            global::net.sf.jni4net.jni.JNIEnv @__env = global::net.sf.jni4net.jni.JNIEnv.Wrap(@__envp);
            global::net.sf.jni4net.utils.JniHandle @__return = default(global::net.sf.jni4net.utils.JniHandle);
            try {
            global::flash.tools.debugger.Value @__real = global::net.sf.jni4net.utils.Convertor.FullJ2C<global::flash.tools.debugger.Value>(@__env, @__obj);
            @__return = global::net.sf.jni4net.utils.Convertor.StrongCp2J(@__real.getTypeName());
            }catch (global::System.Exception __ex){@__env.ThrowExisting(__ex);}
            return @__return;
        }
        
        private static global::net.sf.jni4net.utils.JniHandle getClassName3(global::System.IntPtr @__envp, global::net.sf.jni4net.utils.JniLocalHandle @__obj) {
            // ()Ljava/lang/String;
            // ()Ljava/lang/String;
            global::net.sf.jni4net.jni.JNIEnv @__env = global::net.sf.jni4net.jni.JNIEnv.Wrap(@__envp);
            global::net.sf.jni4net.utils.JniHandle @__return = default(global::net.sf.jni4net.utils.JniHandle);
            try {
            global::flash.tools.debugger.Value @__real = global::net.sf.jni4net.utils.Convertor.FullJ2C<global::flash.tools.debugger.Value>(@__env, @__obj);
            @__return = global::net.sf.jni4net.utils.Convertor.StrongCp2J(@__real.getClassName());
            }catch (global::System.Exception __ex){@__env.ThrowExisting(__ex);}
            return @__return;
        }
        
        private static int getAttributes4(global::System.IntPtr @__envp, global::net.sf.jni4net.utils.JniLocalHandle @__obj) {
            // ()I
            // ()I
            global::net.sf.jni4net.jni.JNIEnv @__env = global::net.sf.jni4net.jni.JNIEnv.Wrap(@__envp);
            int @__return = default(int);
            try {
            global::flash.tools.debugger.Value @__real = global::net.sf.jni4net.utils.Convertor.FullJ2C<global::flash.tools.debugger.Value>(@__env, @__obj);
            @__return = ((int)(@__real.getAttributes()));
            }catch (global::System.Exception __ex){@__env.ThrowExisting(__ex);}
            return @__return;
        }
        
        private static int getMemberCount5(global::System.IntPtr @__envp, global::net.sf.jni4net.utils.JniLocalHandle @__obj, global::net.sf.jni4net.utils.JniLocalHandle par0) {
            // (Lflash/tools/debugger/Session;)I
            // (Lflash/tools/debugger/Session;)I
            global::net.sf.jni4net.jni.JNIEnv @__env = global::net.sf.jni4net.jni.JNIEnv.Wrap(@__envp);
            int @__return = default(int);
            try {
            global::flash.tools.debugger.Value @__real = global::net.sf.jni4net.utils.Convertor.FullJ2C<global::flash.tools.debugger.Value>(@__env, @__obj);
            @__return = ((int)(@__real.getMemberCount(global::net.sf.jni4net.utils.Convertor.FullJ2C<global::flash.tools.debugger.Session>(@__env, par0))));
            }catch (global::System.Exception __ex){@__env.ThrowExisting(__ex);}
            return @__return;
        }
        
        private static int getIsolateId6(global::System.IntPtr @__envp, global::net.sf.jni4net.utils.JniLocalHandle @__obj) {
            // ()I
            // ()I
            global::net.sf.jni4net.jni.JNIEnv @__env = global::net.sf.jni4net.jni.JNIEnv.Wrap(@__envp);
            int @__return = default(int);
            try {
            global::flash.tools.debugger.Value @__real = global::net.sf.jni4net.utils.Convertor.FullJ2C<global::flash.tools.debugger.Value>(@__env, @__obj);
            @__return = ((int)(@__real.getIsolateId()));
            }catch (global::System.Exception __ex){@__env.ThrowExisting(__ex);}
            return @__return;
        }
        
        private static bool isAttributeSet7(global::System.IntPtr @__envp, global::net.sf.jni4net.utils.JniLocalHandle @__obj, int par0) {
            // (I)Z
            // (I)Z
            global::net.sf.jni4net.jni.JNIEnv @__env = global::net.sf.jni4net.jni.JNIEnv.Wrap(@__envp);
            bool @__return = default(bool);
            try {
            global::flash.tools.debugger.Value @__real = global::net.sf.jni4net.utils.Convertor.FullJ2C<global::flash.tools.debugger.Value>(@__env, @__obj);
            @__return = ((bool)(@__real.isAttributeSet(par0)));
            }catch (global::System.Exception __ex){@__env.ThrowExisting(__ex);}
            return @__return;
        }
        
        private static global::net.sf.jni4net.utils.JniHandle getValueAsObject8(global::System.IntPtr @__envp, global::net.sf.jni4net.utils.JniLocalHandle @__obj) {
            // ()Ljava/lang/Object;
            // ()Ljava/lang/Object;
            global::net.sf.jni4net.jni.JNIEnv @__env = global::net.sf.jni4net.jni.JNIEnv.Wrap(@__envp);
            global::net.sf.jni4net.utils.JniHandle @__return = default(global::net.sf.jni4net.utils.JniHandle);
            try {
            global::flash.tools.debugger.Value @__real = global::net.sf.jni4net.utils.Convertor.FullJ2C<global::flash.tools.debugger.Value>(@__env, @__obj);
            @__return = global::net.sf.jni4net.utils.Convertor.FullC2J<global::java.lang.Object>(@__env, @__real.getValueAsObject());
            }catch (global::System.Exception __ex){@__env.ThrowExisting(__ex);}
            return @__return;
        }
        
        private static global::net.sf.jni4net.utils.JniHandle getValueAsString9(global::System.IntPtr @__envp, global::net.sf.jni4net.utils.JniLocalHandle @__obj) {
            // ()Ljava/lang/String;
            // ()Ljava/lang/String;
            global::net.sf.jni4net.jni.JNIEnv @__env = global::net.sf.jni4net.jni.JNIEnv.Wrap(@__envp);
            global::net.sf.jni4net.utils.JniHandle @__return = default(global::net.sf.jni4net.utils.JniHandle);
            try {
            global::flash.tools.debugger.Value @__real = global::net.sf.jni4net.utils.Convertor.FullJ2C<global::flash.tools.debugger.Value>(@__env, @__obj);
            @__return = global::net.sf.jni4net.utils.Convertor.StrongCp2J(@__real.getValueAsString());
            }catch (global::System.Exception __ex){@__env.ThrowExisting(__ex);}
            return @__return;
        }
        
        private static global::net.sf.jni4net.utils.JniHandle getMembers10(global::System.IntPtr @__envp, global::net.sf.jni4net.utils.JniLocalHandle @__obj, global::net.sf.jni4net.utils.JniLocalHandle par0) {
            // (Lflash/tools/debugger/Session;)[Lflash/tools/debugger/Variable;
            // (Lflash/tools/debugger/Session;)[Lflash/tools/debugger/Variable;
            global::net.sf.jni4net.jni.JNIEnv @__env = global::net.sf.jni4net.jni.JNIEnv.Wrap(@__envp);
            global::net.sf.jni4net.utils.JniHandle @__return = default(global::net.sf.jni4net.utils.JniHandle);
            try {
            global::flash.tools.debugger.Value @__real = global::net.sf.jni4net.utils.Convertor.FullJ2C<global::flash.tools.debugger.Value>(@__env, @__obj);
            @__return = global::net.sf.jni4net.utils.Convertor.ArrayFullC2J<flash.tools.debugger.Variable[], global::flash.tools.debugger.Variable>(@__env, @__real.getMembers(global::net.sf.jni4net.utils.Convertor.FullJ2C<global::flash.tools.debugger.Session>(@__env, par0)));
            }catch (global::System.Exception __ex){@__env.ThrowExisting(__ex);}
            return @__return;
        }
        
        private static global::net.sf.jni4net.utils.JniHandle getMemberNamed11(global::System.IntPtr @__envp, global::net.sf.jni4net.utils.JniLocalHandle @__obj, global::net.sf.jni4net.utils.JniLocalHandle par0, global::net.sf.jni4net.utils.JniLocalHandle par1) {
            // (Lflash/tools/debugger/Session;Ljava/lang/String;)Lflash/tools/debugger/Variable;
            // (Lflash/tools/debugger/Session;Ljava/lang/String;)Lflash/tools/debugger/Variable;
            global::net.sf.jni4net.jni.JNIEnv @__env = global::net.sf.jni4net.jni.JNIEnv.Wrap(@__envp);
            global::net.sf.jni4net.utils.JniHandle @__return = default(global::net.sf.jni4net.utils.JniHandle);
            try {
            global::flash.tools.debugger.Value @__real = global::net.sf.jni4net.utils.Convertor.FullJ2C<global::flash.tools.debugger.Value>(@__env, @__obj);
            @__return = global::net.sf.jni4net.utils.Convertor.FullC2J<global::flash.tools.debugger.Variable>(@__env, @__real.getMemberNamed(global::net.sf.jni4net.utils.Convertor.FullJ2C<global::flash.tools.debugger.Session>(@__env, par0), global::net.sf.jni4net.utils.Convertor.StrongJ2CpString(@__env, par1)));
            }catch (global::System.Exception __ex){@__env.ThrowExisting(__ex);}
            return @__return;
        }
        
        private static global::net.sf.jni4net.utils.JniHandle getClassHierarchy12(global::System.IntPtr @__envp, global::net.sf.jni4net.utils.JniLocalHandle @__obj, bool par0) {
            // (Z)[Ljava/lang/String;
            // (Z)[Ljava/lang/String;
            global::net.sf.jni4net.jni.JNIEnv @__env = global::net.sf.jni4net.jni.JNIEnv.Wrap(@__envp);
            global::net.sf.jni4net.utils.JniHandle @__return = default(global::net.sf.jni4net.utils.JniHandle);
            try {
            global::flash.tools.debugger.Value @__real = global::net.sf.jni4net.utils.Convertor.FullJ2C<global::flash.tools.debugger.Value>(@__env, @__obj);
            @__return = global::net.sf.jni4net.utils.Convertor.ArrayStrongCp2J(@__env, @__real.getClassHierarchy(par0));
            }catch (global::System.Exception __ex){@__env.ThrowExisting(__ex);}
            return @__return;
        }
        
        private static global::net.sf.jni4net.utils.JniHandle getPrivateInheritedMembers13(global::System.IntPtr @__envp, global::net.sf.jni4net.utils.JniLocalHandle @__obj) {
            // ()[Lflash/tools/debugger/Variable;
            // ()[Lflash/tools/debugger/Variable;
            global::net.sf.jni4net.jni.JNIEnv @__env = global::net.sf.jni4net.jni.JNIEnv.Wrap(@__envp);
            global::net.sf.jni4net.utils.JniHandle @__return = default(global::net.sf.jni4net.utils.JniHandle);
            try {
            global::flash.tools.debugger.Value @__real = global::net.sf.jni4net.utils.Convertor.FullJ2C<global::flash.tools.debugger.Value>(@__env, @__obj);
            @__return = global::net.sf.jni4net.utils.Convertor.ArrayFullC2J<flash.tools.debugger.Variable[], global::flash.tools.debugger.Variable>(@__env, @__real.getPrivateInheritedMembers());
            }catch (global::System.Exception __ex){@__env.ThrowExisting(__ex);}
            return @__return;
        }
        
        private static global::net.sf.jni4net.utils.JniHandle getPrivateInheritedMemberNamed14(global::System.IntPtr @__envp, global::net.sf.jni4net.utils.JniLocalHandle @__obj, global::net.sf.jni4net.utils.JniLocalHandle par0) {
            // (Ljava/lang/String;)[Lflash/tools/debugger/Variable;
            // (Ljava/lang/String;)[Lflash/tools/debugger/Variable;
            global::net.sf.jni4net.jni.JNIEnv @__env = global::net.sf.jni4net.jni.JNIEnv.Wrap(@__envp);
            global::net.sf.jni4net.utils.JniHandle @__return = default(global::net.sf.jni4net.utils.JniHandle);
            try {
            global::flash.tools.debugger.Value @__real = global::net.sf.jni4net.utils.Convertor.FullJ2C<global::flash.tools.debugger.Value>(@__env, @__obj);
            @__return = global::net.sf.jni4net.utils.Convertor.ArrayFullC2J<flash.tools.debugger.Variable[], global::flash.tools.debugger.Variable>(@__env, @__real.getPrivateInheritedMemberNamed(global::net.sf.jni4net.utils.Convertor.StrongJ2CpString(@__env, par0)));
            }catch (global::System.Exception __ex){@__env.ThrowExisting(__ex);}
            return @__return;
        }
        
        new internal sealed class ContructionHelper : global::net.sf.jni4net.utils.IConstructionHelper {
            
            public global::net.sf.jni4net.jni.IJvmProxy CreateProxy(global::net.sf.jni4net.jni.JNIEnv @__env) {
                return new global::flash.tools.debugger.@__Value(@__env);
            }
        }
    }
    #endregion
}
