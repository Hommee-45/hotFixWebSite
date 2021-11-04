local Debug = CS.UnityEngine.Debug

Log = {}
local this = Log
function this.Info(...)
    local msg = this.GetMessage(...)
    Debug.Log(msg)
end
function this.Warning(...)
    local msg = this.GetMessage(...)
    Debug.LogWarning(msg)
end
function this.Error(...)
    local msg = this.GetMessage(...)
    Debug.LogError(msg)
end
function this.GetMessage(...)
    local msg = ""
    for k, v in ipairs({...}) do
        if v == nil then
            msg = msg .. "<nil>" .. "\n"
        elseif type(v) == "table" then
            msg = msg .. "\n" .. this.GetTableMsg(v, 0) .. "\n"
        else
            msg = msg .. tostring(v) .. "  "
        end
    end
    return msg
end
function this.GetTableMsg(tab, space)
    local space1 = ""
    local space2 = ""
    for i = 1, space do
        space1 = space1 .. "    "
    end
    space2 = space1 .. "    "
    local msg = "{"
    for k, v in pairs(tab) do
        if v == nil then
            msg = msg .. "<nil>" .. "\n"
        elseif type(v) == "table" then
            msg = msg .. "\n" .. space2 .. tostring(k) .. " : " .. this.GetTableMsg(v, space + 1)
        else
            msg = msg .. "\n" .. space2 .. tostring(k) .. " : " .. tostring(v)
        end
    end
    msg = msg .. "\n" .. space1 .. "}"
    return msg
end